using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharControllers : MonoBehaviour {

    [Header("Moving")]
    [SerializeField] private float m_groundedSpeed = 15;

    [Header("Airborne")]
    [SerializeField] private float m_baseGravityMultiplier = 5;
    [SerializeField] private float m_airSpeedModifier = 1.5f;

    [Header("Ground Check")]
    [SerializeField] private float m_groundCheckDistance = 0.2f;
    [SerializeField] private float m_ungroundDelay = 0.2f;
    [SerializeField] private float m_sphereCastRadius = 0.35f;
    [SerializeField] private LayerMask m_LayerGround;

    /*[Header("Glide")]
    public float m_baseGravityFactorGlide = 10;
    [SerializeField] private float m_forceGlide = -1.5f;

    [Header("Jump")]
    [SerializeField] private float m_jumpHeight = 20;
    [SerializeField] private float m_superJumpMultiplicator = 1.5f;

    [Header("UI")]
    //[SerializeField] private GameObject m_pauseMenu;

    /* [Header("Visual Effects")]
    [SerializeField] private GameObject m_GlideEffect;
    [SerializeField] private GameObject m_jumpDustEffect;
    [SerializeField] private GameObject m_jumpTrailEffect;
    [SerializeField] private GameObject m_swapAliveEffect;
    [SerializeField] private GameObject m_swapDeadEffect;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] m_walkClips;
    [SerializeField] private AudioClip m_aliveJumpClip;
    [SerializeField] private AudioClip m_deadJumpClip;
    [SerializeField] private AudioClip m_superJumpClip;
    [SerializeField] private AudioClip m_glideClip;
    [SerializeField] private AudioClip m_hurtClip;
    [SerializeField] private AudioClip m_splashClip;*/

    /*[SerializeField] private GameObject m_walkSound;
    [SerializeField] private GameObject m_jumpSound;
    [SerializeField] private GameObject m_superJumpSound;
    [SerializeField] private GameObject m_GlideSound;*/

    //PUBLIC
    [Header("State")]
    public bool m_isGrounded;
    public bool m_isColliding;
    public bool m_isWalking;
    public bool m_isGliding;
    public bool m_isJumping;
    public bool m_isSuperJumping;
    public bool m_useGravity = true;
    public bool m_canMove = true;
    [Header("Others")]
    public float m_moveSpeed;
    public float m_VerticalVelocity;
    public float m_gravityFactorGlide;

    //PRIVATE
    private CharacterController m_characController;
    private AudioSource m_audioSource;

    /*private Animator m_animatorAlive;
    private Animator m_animatorDead;*/
    private GameObject m_JumpEffectTemp = null;
    //Camera
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    //Movement
    private Vector3 m_Move;
    private float m_gravityMultiplier;
    //INPUT
    private float m_vInput;
    private float m_hInput;
    //private bool m_jumpInput;
    //private bool m_superJumpInput;
    //private bool m_glideInput;
    private bool m_swapInput = true;


    /*---------------------------------------------------------*/
    /*------------------------START----------------------------*/
    /*---------------------------------------------------------*/
    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
            m_Cam = Camera.main.transform;
        else
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);

        StopAllCoroutines();

        //m_pauseMenu.SetActive(false);
        //
        m_characController = GetComponent<CharacterController>();
        //
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.loop = false;
        m_audioSource.spatialBlend = 0;
        //m_audioSource.clip = m_glideClip;
        //
        m_gravityMultiplier = m_baseGravityMultiplier;
        //m_gravityFactorGlide = m_baseGravityFactorGlide;
        //
        /*if (transform.Find("Mesh_Char_Alive").GetComponent<Animator>() != null)
            m_animatorAlive = transform.Find("Mesh_Char_Alive").GetComponent<Animator>();
        if (transform.Find("Mesh_Char_Dead").GetComponent<Animator>() != null)
            m_animatorDead = transform.Find("Mesh_Char_Dead").GetComponent<Animator>();*/
    }

    /*---------------------------------------------------------*/
    /*----------------------UPDATES----------------------------*/
    /*---------------------------------------------------------*/
    private void Update()
    {
        GetInput();
        //CheckGroundStatus();
        Anim();
    }

    private void FixedUpdate()
    {
        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = m_vInput * m_CamForward * m_moveSpeed * Time.deltaTime + m_hInput * m_Cam.right * m_moveSpeed * Time.deltaTime;
        }
        else
            Debug.LogError("No main Camera");

        Debug.DrawRay(transform.position, m_Move, Color.magenta);

        //TURN
        if (m_canMove)
        {
            if (m_Move.magnitude == 0)
                transform.rotation = transform.rotation;
            else
                transform.rotation = Quaternion.LookRotation(m_Move);
        }

        //ACTIONS
        if (m_characController.isGrounded /* m_isGrounded*/)
        {
            //GROUNDED
            Jump();

            m_moveSpeed = m_groundedSpeed;
        }
        else
        {
            //AIRBORNE
            Glide();

            m_moveSpeed = m_groundedSpeed / m_airSpeedModifier;

            if (m_characController.velocity.y < 0)
            {
                if(m_JumpEffectTemp != null)
                    Destroy(m_JumpEffectTemp);
                if(m_isSuperJumping)
                    m_isSuperJumping = false;
                if (m_isJumping)
                    m_isJumping = false;
            }
        }
        Gravity();

        m_isWalking = (Mathf.Abs(m_Move.x) > 0 && Mathf.Abs(m_Move.z) > 0) ? true : false;

        if(m_canMove)
            m_characController.Move(m_Move);

    }

    /*---------------------------------------------------------*/
    /*------------------------INPUT----------------------------*/
    /*---------------------------------------------------------*/
    private void GetInput()
    {
        //Movement
        m_hInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(m_hInput) > 1)
            m_hInput = 1;
        m_vInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(m_vInput) > 1)
            m_vInput = 1;

        //Jump
        /*if (Input.GetButtonDown("Jump") || Input.GetButton("Jump"))
        {
            m_jumpInput = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            m_jumpInput = false;
        }*/

        //Ability            
        /*if (Input.GetButtonDown("Ability") || Input.GetButtonDown("X360_RBumper") || Input.GetButton("X360_RBumper"))
        {
            if (m_worldState == WorldState.Alive)
                m_superJumpInput = true;
            else
                m_glideInput = true;
        }
        if (Input.GetButtonUp("Ability") || Input.GetButtonUp("X360_A") || Input.GetButtonUp("X360_RBumper"))
        {
            if (m_worldState == WorldState.Alive)
                m_superJumpInput = false;
            else
                m_glideInput = false;
        }*/

        //Swap
        if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("X360_LBumper"))
        {
            m_swapInput = !m_swapInput;
        }

        //Start
        /*if (Input.GetButtonDown("X360_Start"))
        {
            m_canMove = !m_canMove;
            m_pauseMenu.SetActive(!m_canMove);
        }*/
    }

    /*---------------------------------------------------------*/
    /*----------------------MOUVEMENT--------------------------*/
    /*---------------------------------------------------------*/
    void Jump()
    {
        /*if (m_superJumpInput && !m_isSuperJumping)
        {
            m_isSuperJumping = true;

            //Sound
            //m_audioSource.PlayOneShot(m_superJumpClip);
            //Level.AddSound(m_superJumpSound, transform.position);

            m_VerticalVelocity = m_jumpHeight * m_superJumpMultiplicator;
        }
        else if (m_jumpInput && !m_isJumping)
        {
            m_isJumping = true;
            m_VerticalVelocity = m_jumpHeight;
        }
        else
            m_VerticalVelocity = 0f;*/
    }

    private void Glide()
    {
        /*if (m_glideInput && m_characController.velocity.y <= 0)
        {
            //FX
           // m_GlideEffect.SetActive(true);
            //Sound
            if (!m_audioSource.isPlaying)
                m_audioSource.Play();

            if (m_useGravity)
            {
                m_VerticalVelocity = m_forceGlide;
                m_gravityMultiplier = m_baseGravityMultiplier / m_gravityFactorGlide;
            }

            m_isGliding = true;
        }
        else
        {
            //FX
            //m_GlideEffect.SetActive(false);
            //Sound
            if (m_audioSource.isPlaying && m_isGliding)
                m_audioSource.Stop();

            m_gravityMultiplier = m_baseGravityMultiplier;

            m_useGravity = true;
            m_isGliding = false;
        }*/
    }

    void Gravity()
    {
        if (m_useGravity)
        {
            //Gravity
            m_VerticalVelocity += Physics.gravity.y * m_gravityMultiplier * Time.fixedDeltaTime;
            m_Move.y += m_VerticalVelocity * Time.fixedDeltaTime;
        }
    }

    /*---------------------------------------------------------*/
    /*----------------------ANIMATION--------------------------*/
    /*---------------------------------------------------------*/
    private void Anim()
    {
        /*if(m_worldState == WorldState.Alive)
        {
            // update the animator parameters
            m_animatorAlive.SetBool("CanMove", m_canMove);
            m_animatorAlive.SetBool("Superjump", m_isSuperJumping);
            m_animatorAlive.SetFloat("VerticalVelocity", m_VerticalVelocity);
            m_animatorAlive.SetBool("Walking", m_isWalking);
            m_animatorAlive.SetBool("Grounded", /*m_isGrounded m_characController.isGrounded);
        }
        else
        {
            m_animatorDead.SetBool("CanMove", m_canMove);
            m_animatorDead.SetBool("IsGliding", m_isGliding);
            m_animatorDead.SetBool("IsWalking", m_isWalking);
            m_animatorDead.SetBool("Grounded", /*m_isGroundedm_characController.isGrounded );
        }*/
    }

    /*---------------------------------------------------------*/
    /*------------------------OTHER----------------------------*/
    /*---------------------------------------------------------*/
    //SWITCH
    /*public void SwitchChar(WorldState charState)
    {
        switch (charState)
        {
            case WorldState.Alive:
                //
                Level.AddFx(m_swapAliveEffect, transform.position + transform.up * 0.5f, Quaternion.identity, gameObject);

                m_worldState = WorldState.Alive;
                m_GlideEffect.SetActive(false);
                m_glideInput = false;
                transform.Find("Mesh_Char_Alive").gameObject.SetActive(true);
                transform.Find("Mesh_Char_Dead").gameObject.SetActive(false);
                break;
            case WorldState.Dead:
                //
                Level.AddFx(m_swapDeadEffect, transform.position + transform.up*0.5f, Quaternion.identity, gameObject);

                m_worldState = WorldState.Dead;
                m_superJumpInput = false;
                transform.Find("Mesh_Char_Alive").gameObject.SetActive(false);
                transform.Find("Mesh_Char_Dead").gameObject.SetActive(true);
                break;
        }
    }*/

    private AudioClip RandomSound(AudioClip[] clipTab)
    {
        return clipTab[Random.Range(0, clipTab.Length)];
    }

    //GROUNDED
    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.4f), transform.position + (Vector3.up * 0.4f) + (Vector3.down * m_groundCheckDistance));
        Debug.DrawRay(transform.position + (Vector3.up * 0.4f), Vector3.down * m_groundCheckDistance);
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        
        if (Physics.SphereCast(transform.position + Vector3.up * 0.4f, m_sphereCastRadius, Vector3.down, out hitInfo, m_groundCheckDistance, m_LayerGround))
        //if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_groundCheckDistance, m_LayerGround))
        {
            m_isGrounded = true;
        }
        else
        {
            StartCoroutine(UngroundDelay());
        }
    }
/*
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3.up * 0.4f), m_sphereCastRadius);
        Gizmos.DrawWireSphere(transform.position + (Vector3.down * m_groundCheckDistance), m_sphereCastRadius);
    }
#endif*/

    //COROUTINES
    private IEnumerator UngroundDelay()
    {
        yield return new WaitForSeconds(m_ungroundDelay);
        m_isGrounded = false;
    }

    //TRIGGERS AND COLLISIONS
    /*private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "DamageZone")
        {
            m_audioSource.PlayOneShot(m_hurtClip, 1);
        }
        if(other.gameObject.layer == 4) //Water
        {
            m_audioSource.PlayOneShot(m_splashClip, 1);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
    }*/

    private void OnCollisionStay(Collision collision)
    {
        m_isColliding = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        m_isColliding = false;
    }
}
