﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsController : MonoBehaviour {

    [SerializeField] private Transform m_upperBody;

    [Header("Shooting")]
    //[SerializeField] private RaycastManager m_raycastManager;
    //[SerializeField] private Transform m_Gun;
    //[SerializeField] private GameObject m_HitSphere;
    //[SerializeField] private LayerMask m_shootLayer;
    //[SerializeField] private Transform m_shootPoint;
    //[SerializeField] private float m_bulletSpeed;
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
	}
	
	// Update is called once per frame
	void Update () 
	{
		CharInput();

        //Repair
        if(m_shiphealth.m_nbImpact > 0)
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
        /*if (Mathf.Abs(m_inputH) > 1)
            m_inputH = 1;*/
        m_inputV = Input.GetAxis("Vertical");
        /*if (Mathf.Abs(m_inputV) > 1)
            m_inputV = 1;*/
        
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
			m_ship.OrderSailsUp();
		}
		if(Input.GetButtonDown("X360_X")){	//gauche
            m_ship.OrderSailsDown();
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
            //m_ship.m_canonsLeft.BroadcastMessage("CanonShoot", SendMessageOptions.DontRequireReceiver);
            m_Canons.ShootCanon(m_Canons.m_canonsLeft);
            m_Canons.ReloadCanon(m_Canons.m_canonsLeft);
        }
        else{
            //m_ship.m_canonsRight.BroadcastMessage("CanonShoot", SendMessageOptions.DontRequireReceiver);
            m_Canons.ShootCanon(m_Canons.m_canonsRight);
            m_Canons.ReloadCanon(m_Canons.m_canonsRight);
        }

        yield return new WaitForSeconds(m_ShootDelay);

        canShootLeft = true;
        canShootRight = true;
    }

    private void RaycastImpact()
	{
		RaycastHit hitInfo;

		if(Physics.Raycast(m_camera.position + (m_camera.forward*1), m_camera.forward, out hitInfo, RaycastMaxDistance, m_raycastLayers, QueryTriggerInteraction.Collide)){
			isRaycastImpactHit = true;

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

        for (int i = (int)m_repairSlider.value; i < m_repairSlider.maxValue; i++)
        {
            m_repairSlider.value ++;
            yield return new WaitForSeconds(m_repairDuration/m_repairSlider.maxValue);
        }
        
        m_shiphealth.IncreaseLife(0, (int)((m_HealPercentage/100) * m_shiphealth.m_lifebars[0].MaxlifePoints));
        impact.DisableImpact(true);
        impact.m_parentZone.RemoveImpactUI();
        m_repairSlider.value = 0;
        isRepairing = false;
    }

    private IEnumerator KeepRepairValue(){

        yield return new WaitForSeconds(m_KeepValueDuration);
        m_repairSlider.value = 0;
    }
}
