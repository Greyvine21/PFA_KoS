using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsController : MonoBehaviour {

    [Header("Shooting")]
    //[SerializeField] private RaycastManager m_raycastManager;
    //[SerializeField] private Transform m_Gun;
    //[SerializeField] private GameObject m_HitSphere;
    //[SerializeField] private LayerMask m_shootLayer;
    //[SerializeField] private Transform m_shootPoint;
    //[SerializeField] private float m_bulletSpeed;
    [SerializeField] private float m_ShootDelay;
    //[SerializeField] private GameObject m_Bullet;

    [Header("Monitoring")]
	public bool m_isInteracting;
    public bool canInteract = true, canShootLeft = true, canShootRight = true;
	[HideInInspector] public float m_inputH;
	[HideInInspector] public float m_inputV;
    [HideInInspector] public AgentController_Receiver m_receiver;
    private PlayerShipBehaviour m_ship;
    private CanonManager m_Canons;



	// Use this for initialization
	void Start () {
        //m_ship = transform.GetComponentInParent<PlayerShipBehaviour>();
        m_Canons = transform.GetComponentInParent<CanonManager>();
        m_receiver = GetComponent<AgentController_Receiver>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		CharInput();
	}

	private void CharInput(){
        //MVT
        m_inputH = Input.GetAxis("Horizontal");
        if (Mathf.Abs(m_inputH) > 1)
            m_inputH = 1;
        m_inputV = Input.GetAxis("Vertical");
        if (Mathf.Abs(m_inputV) > 1)
            m_inputV = 1;

        //
        if(Input.GetButtonUp("X360_Start") || Input.GetKeyDown(KeyCode.Escape)){
#if UNITY_EDITOR
            Debug.Break();
#else
            Application.Quit();
#endif
        }

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
}
