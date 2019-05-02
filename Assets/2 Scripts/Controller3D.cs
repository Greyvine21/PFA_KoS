using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller3D : MonoBehaviour {
	
    [Header("References")]
    [SerializeField] public Transform m_Body;

    [Header("Moving")]
    [SerializeField] private float m_moveSpeed = 15;
	[SerializeField] private LayerMask m_LayerGround;
    public Vector3 velocityPlayer;

    [Header("Shooting")]
    [SerializeField] private RaycastManager m_raycastManager;
    [SerializeField] private Transform m_Gun;
    [SerializeField] private GameObject m_HitSphere;
    [SerializeField] private LayerMask m_shootLayer;
    [SerializeField] private Transform m_shootPoint;
    [SerializeField] private float m_bulletSpeed;
    [SerializeField] private float m_ShootDelay;
    [SerializeField] private GameObject m_Bullet;

    
    [Header("Monitoring")]
    public Vector3 m_CamForward;
	public Vector3 m_Move;
    public bool isMoving, isMovingOLD = false;
    public bool isGrounded;

	[HideInInspector] public Rigidbody m_rbPlayer;
	private float m_inputH;
	private float m_inputV;
	/*[HideInInspector]*/ public bool m_isInteracting;
    private FixedJoint m_fixedJointTemp;
    private Transform m_Cam;
    private LineRenderer m_lineGun;
    private FloatingShip m_ship;

    public bool canMove = true, canInteract = true, canShoot = true;

	void Start () {
		m_rbPlayer = GetComponent<Rigidbody>();       
        m_lineGun = GetComponent<LineRenderer>();
        m_ship = transform.GetComponentInParent<FloatingShip>();
		
		if (Camera.main != null)
            m_Cam = Camera.main.transform;
        else
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
	}
	

	void Update () 
	{
		CharInput();
        DetectingGround();
        //Aim();
	}   

	private void FixedUpdate()
    {        
        //Calcutate MVT
        if (m_Cam != null)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = m_inputV * m_CamForward * m_moveSpeed * Time.deltaTime + m_inputH * m_Cam.right * m_moveSpeed * Time.deltaTime;
            //m_Move = Vector3.Scale(m_Move,  new Vector3(1, 0, 1));
            //
            Debug.DrawRay(transform.transform.position, m_Move*10, Color.blue);
        }
        else
            Debug.LogError("No main Camera");
        
        isMoving = (m_Move.magnitude > 0);

        //FIX PLAYER
        if(isMoving != isMovingOLD){
            if (isMoving){
                UnfixPlayer();
            }
            else if(isGrounded)
                FixPlayer();
            isMovingOLD = isMoving;
        }

        //MVT
        if(canMove){
            m_rbPlayer.MovePosition(m_rbPlayer.position + m_Move);
        }
        velocityPlayer = m_rbPlayer.velocity;
        
        //ROTATION           
        if(isMoving && canMove){
            m_Body.forward = m_Move;
        }
        transform.rotation = transform.parent.rotation;
	}

    private void Aim(){
        m_Gun.LookAt(m_raycastManager.m_hitPoint);
        m_lineGun.SetPosition(0, m_shootPoint.position);

        RaycastHit hit;
        if(Physics.Raycast(m_shootPoint.position, m_Gun.forward, out hit, m_raycastManager.RaycastMaxDistance, m_shootLayer)){
            m_lineGun.SetPosition(1, hit.point);
            m_HitSphere.transform.position = hit.point;
        }
        else{
            m_lineGun.SetPosition(1, m_raycastManager.m_hitPoint.position);
            m_HitSphere.transform.position = m_lineGun.GetPosition(1);
        }
    }

    private IEnumerator Shoot(bool isLeft){
        canShoot = false;
        //print("shoot");
        //GameObject bullet = Instantiate(m_Bullet, m_shootPoint.position, m_shootPoint.rotation);
        //bullet.name = "Bullet";
        //bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * m_bulletSpeed, ForceMode.Impulse);
        if(isLeft)
            m_ship.m_canonsleft.BroadcastMessage("CanonShoot", SendMessageOptions.DontRequireReceiver);
        else
            m_ship.m_canonsRight.BroadcastMessage("CanonShoot", SendMessageOptions.DontRequireReceiver);

        yield return new WaitForSeconds(m_ShootDelay);

        canShoot = true;
    }

    public void FixPlayer()
    {
        if(m_fixedJointTemp == null){
            m_fixedJointTemp = gameObject.AddComponent<FixedJoint>();
            m_fixedJointTemp.connectedBody = transform.parent.GetComponentInParent<Rigidbody>();
            //m_rbPlayer.isKinematic = true;
            //print("Add Joint");
        }
    }

    public void UnfixPlayer(){
        if(m_fixedJointTemp != null){
            Destroy(m_fixedJointTemp);
            //m_rbPlayer.isKinematic = false;
            //print("remove Joint");
        }
    }

    public void LockPlayerOnPoint(Transform point){
    
        UnfixPlayer();
        transform.position = point.position;
        transform.rotation = point.rotation;
        m_Body.transform.rotation = point.rotation;
        FixPlayer();
        canMove = false;
		m_isInteracting = false;
    }
    public void UnlockLockPlayerFromPoint(Transform point){
        canMove = true;
        m_isInteracting = false;
        transform.rotation = point.rotation;
        m_Body.transform.rotation = point.rotation;
        UnfixPlayer();
    }

    private void DetectingGround()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        //Debug.DrawLine(transform.position + (Vector3.up * 0.4f), transform.position + (Vector3.up * 0.4f) + (Vector3.down * m_groundCheckDistance));
        Debug.DrawRay(transform.position + (Vector3.up * 0.1f), Vector3.down * 0.3f, Color.magenta);
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        
        //if (Physics.SphereCast(transform.position + Vector3.up * 0.1f, 0.35f, Vector3.down, out hitInfo, 0.3f, m_LayerGround))
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 0.3f, m_LayerGround))
        {
            isGrounded = true;
            //Debug.DrawRay(transform.position, hitInfo.normal,Color.blue, 2);
            //print(hitInfo.collider.gameObject.name + "  /  " + hitInfo.point);
        }
        else{
            isGrounded = false;
        }
    }

	private void CharInput(){
        //MVT
        m_inputH = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(m_inputH) > 1)
            m_inputH = 1;
        m_inputV = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(m_inputV) > 1)
            m_inputV = 1;

        if(Input.GetButtonUp("X360_Start")){
            transform.position = transform.parent.position;
        }

        //INTERACT
        if(Input.GetButtonDown("X360_A") && canInteract){
            m_isInteracting = true;
        }
        if(Input.GetButtonUp("X360_A") && canInteract){
            m_isInteracting = false;
        }

        //SHOOT
        if(Input.GetAxisRaw("X360_Triggers") < 0 && canShoot){
            StartCoroutine("Shoot", true);
        }
        if(Input.GetAxisRaw("X360_Triggers") > 0 && canShoot){
            StartCoroutine("Shoot", false);
        }
	}
}
