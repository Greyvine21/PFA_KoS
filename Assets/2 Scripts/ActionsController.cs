using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsController : MonoBehaviour {

    [SerializeField] private MinionsManager m_minions;

    [Header("Sails")]
    [SerializeField] private Transform[] m_sailsPoints;


    [Header("Shooting")]
    //[SerializeField] private RaycastManager m_raycastManager;
    //[SerializeField] private Transform m_Gun;
    //[SerializeField] private GameObject m_HitSphere;
    //[SerializeField] private LayerMask m_shootLayer;
    //[SerializeField] private Transform m_shootPoint;
    //[SerializeField] private float m_shootShakeDuration = 0.5f;
    //[SerializeField] private float m_shootShakeMagnitude = 0.5f;
    [SerializeField] private float m_ShootDelay;
    //[SerializeField] private GameObject m_Bullet;

    [Header("Repair")]
	public float m_repairDuration = 4;
	public float m_KeepValueDuration = 4;
	[Range(0,100)] public float m_HealPercentage = 10;
	public float RaycastMaxDistance = 100;
	public LayerMask m_raycastLayers; //19
	public GameObject m_repairText;
	public Slider m_repairSlider;

    [Header("Monitoring")]
	public bool isRaycastImpactHit;
    public bool isRepairing;
	public bool m_isInteracting;
    public bool canInteract = true, canShootLeft = true, canShootRight = true;
	[HideInInspector] public float m_inputH;
	[HideInInspector] public float m_inputV;
    [HideInInspector] public AgentController_Receiver m_receiver;

    private Impact m_impactSelected;
    private PlayerShipBehaviour m_ship;
    private CanonManager m_Canons;
	private Animator m_animRepairText;
	public Animator m_animPlayer;
    private healthManager m_shiphealth;
    private Transform m_camera;


	// Use this for initialization
	void Start () {
        m_ship = transform.GetComponentInParent<PlayerShipBehaviour>();
        m_Canons = m_ship.GetComponentInChildren<CanonManager>();
        m_shiphealth = m_ship.GetComponentInChildren<healthManager>();
        //
        m_camera = Camera.main.transform;
		m_animRepairText = m_repairText.GetComponent<Animator>();
        m_repairSlider.value = 0;
        //
        m_receiver = GetComponent<AgentController_Receiver>();
        m_animPlayer = m_receiver.m_animator;
	}
	
	// Update is called once per frame
	void Update () 
	{
		CharInput();

        //
        if(m_animPlayer){
			m_animPlayer.SetBool("canMove" , m_receiver.m_ControllerAgent.m_agentCanMove);
        }
        //Repair
        //if(m_shiphealth.m_totalNbImpact > 0)
            RaycastImpact();
	}

	private void CharInput(){

        //Quit / Pause
        if(Input.GetButtonUp("X360_Start") || Input.GetKeyDown(KeyCode.Escape)){
#if UNITY_EDITOR
            Debug.Break();
#else
            Application.Quit();
#endif
        }

        //MVT
        m_inputH = Input.GetAxis("Horizontal");
        if (Mathf.Abs(m_inputH) > 1)
            m_inputH = 1;
        m_inputV = Input.GetAxis("Vertical");
        if (Mathf.Abs(m_inputV) > 1)
            m_inputV = 1;
        
        //INTERACT
        if(Input.GetButtonDown("X360_A") && canInteract){
            m_isInteracting = true;
        }
        if(Input.GetButtonUp("X360_A") && canInteract){
            m_isInteracting = false;
        }

        //SHOOT
        if(Input.GetAxisRaw("X360_LTrigger") > 0 && canShootLeft){
            StartCoroutine("Shoot", true);
        }
        if(Input.GetAxisRaw("X360_RTrigger") > 0 && canShootRight){
            StartCoroutine("Shoot", false);
        }

        //REPAIR
        if(Input.GetButtonDown("X360_Y") && isRaycastImpactHit){
            StopCoroutine("KeepRepairValue");
            StartCoroutine("Repair", m_impactSelected);
        }
        if(isRepairing){
            if(Input.GetButtonUp("X360_Y") || !isRaycastImpactHit){
                StopCoroutine("Repair");
                StartCoroutine("KeepRepairValue");
                isRepairing = false;
            }
        }

        //STEER
        if (m_inputH < 0 && m_ship.m_rudderInteractZone.isZoneActive())  //left
        {
			m_ship.SteerLeft();
        }
        else if (m_inputH > 0 && m_ship.m_rudderInteractZone.isZoneActive()) //right
        {
			m_ship.SteerRight();
        }

        //ANCHOR
		if(m_isInteracting && m_ship.m_anchorZone.playerInZone){
			m_ship.Anchor();
		}

        //SAILS
		if(Input.GetButtonDown("X360_B")){	//droite
            if(m_ship.m_sailsManager.OrderSailsUp()){                       //Order
                    m_minions.SendUnitToMultiplePos(m_sailsPoints, m_minions.m_sailorsSailsTab, 1);      //Unit
                    StartCoroutine(m_minions.SendActionDone(m_minions.m_sailorsSailsTab, 5));
            }
		}
		else if(Input.GetButtonDown("X360_X")){	//gauche
            if(m_ship.m_sailsManager.OrderSailsDown()){                     //Order
                    m_minions.SendUnitToMultiplePos(m_sailsPoints, m_minions.m_sailorsSailsTab, 1);      //Unit
                    StartCoroutine(m_minions.SendActionDone(m_minions.m_sailorsSailsTab, 5));
            }
		}
	}

	// private void Aim(){
    //     m_Gun.LookAt(m_raycastManager.m_hitPoint);
    //     m_lineGun.SetPosition(0, m_shootPoint.position);

    //     RaycastHit hit;
    //     if(Physics.Raycast(m_shootPoint.position, m_Gun.forward, out hit, m_raycastManager.RaycastMaxDistance, m_shootLayer)){
    //         m_lineGun.SetPosition(1, hit.point);
    //         m_HitSphere.transform.position = hit.point;
    //     }
    //     else{
    //         m_lineGun.SetPosition(1, m_raycastManager.m_hitPoint.position);
    //         m_HitSphere.transform.position = m_lineGun.GetPosition(1);
    //     }
    // }

    private IEnumerator Shoot(bool isLeft){
        canShootLeft = !isLeft;
        canShootRight = isLeft;

        //print("shoot");
        //GameObject bullet = Instantiate(m_Bullet, m_shootPoint.position, m_shootPoint.rotation);
        //bullet.name = "Bullet";
        //bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * m_bulletSpeed, ForceMode.Impulse);

        if(isLeft){
            if(m_Canons.ShootCanon(m_Canons.m_canonsLeft, m_minions.m_gunnersLeft)){        
                PlayerShootAnim(isLeft);
            }
            m_Canons.ReloadCanon(m_Canons.m_canonsLeft);
            //yield return new WaitForSeconds(0.6f);
        }
        else{
            if(m_Canons.ShootCanon(m_Canons.m_canonsRight, m_minions.m_gunnersRight)){        
                PlayerShootAnim(isLeft);
            }
            m_Canons.ReloadCanon(m_Canons.m_canonsRight);
            //yield return new WaitForSeconds(0.6f);
        }

        yield return new WaitForSeconds(1f);

        m_receiver.m_ControllerAgent.m_agentCanMove = true;

        yield return new WaitForSeconds(m_ShootDelay);

        canShootLeft = true;
        canShootRight = true;
    }

    private void PlayerShootAnim(bool left){
        m_receiver.m_ControllerAgent.m_agentCanMove = false;
        m_receiver.m_ControllerAgent.m_agent.velocity = Vector3.zero;

        m_receiver.m_ControllerAgent.transform.rotation = m_ship.transform.rotation * Quaternion.Euler(0,left? -90 : 90,0);

        //StartCoroutine(CameraShake.Shake(m_shootShakeDuration, m_shootShakeMagnitude));
        m_animPlayer.Play("Charge");
    }


    private void RaycastImpact()
	{
		RaycastHit hitInfo;

		if(Physics.Raycast(m_camera.position + (m_camera.forward*1), m_camera.forward, out hitInfo, RaycastMaxDistance, m_raycastLayers, QueryTriggerInteraction.Collide)){
			isRaycastImpactHit = true;
            //print(hitInfo.collider.name);
			switch (hitInfo.collider.gameObject.layer)
			{
				case 19:
                    m_impactSelected = hitInfo.collider.gameObject.GetComponent<Impact>();
					m_animRepairText.SetBool("InteractAnim",true);
					m_repairText.transform.position = Camera.main.WorldToScreenPoint(hitInfo.point);
				break;
				default:
					Debug.LogWarning("Raycast camera");
				break;
			}
		}
		else{
            m_impactSelected = null;
			m_animRepairText.SetBool("InteractAnim",false);
			isRaycastImpactHit = false;
		}
	}

    private IEnumerator Repair(Impact impact){
        isRepairing = true;
        m_minions.SendUnitToOnePos(m_impactSelected.transform.position, m_minions.m_sailorsRepairTab, 3);

        for (int i = (int)m_repairSlider.value; i < m_repairSlider.maxValue; i++)
        {
            m_repairSlider.value ++;
            yield return new WaitForSeconds(m_repairDuration/m_repairSlider.maxValue);
        }
        
        //
        StartCoroutine(m_minions.SendActionDone(m_minions.m_sailorsRepairTab));
        //
        m_shiphealth.IncreaseLife(0, (int)((m_HealPercentage/100) * m_shiphealth.m_lifebar.MaxlifePoints));
        m_shiphealth.m_totalNbImpact --;
        //
        impact.DisableImpact(true);
        impact.m_parentZone.RemoveImpactUI();
        m_repairSlider.value = 0;
        //
        isRepairing = false;
    }

    private IEnumerator KeepRepairValue()
    {
        yield return new WaitForSeconds(m_KeepValueDuration);
        m_repairSlider.value = 0;
    }
}
