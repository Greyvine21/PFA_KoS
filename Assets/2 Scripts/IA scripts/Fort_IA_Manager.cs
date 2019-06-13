using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fort_IA_Manager : MonoBehaviour {

	public GameObject m_skull;
	public bool canShoot;
	public Transform m_visor;
	public float RangeRotationRight = 30;
	public float RangeFactor = 40.5f;
	public float m_rotationOffset = 10;
    public GameObject imageUI;
	public bool PlayerInRange;
	public float distanceFromTarget;
	private Transform m_Target;
	private Vector3 m_TargetGlobalPos;
	private CanonManager m_canonsManager;
	private healthManager m_healthManager;
	private Mortar m_mortar;


	// Use this for initialization
	void Start () {
		m_canonsManager = GetComponentInChildren<CanonManager>();
		m_mortar = GetComponentInChildren<Mortar>();
		m_healthManager = GetComponentInChildren<healthManager>();
		if(m_healthManager)
			m_healthManager.OnLifeReachZero += Defeat;

		m_skull.SetActive(false);

		if(GameObject.FindGameObjectWithTag("PlayerShip")){
			m_Target = GameObject.FindGameObjectWithTag("PlayerShip").transform;
		}
	}

	void OnDisable()
	{
		if(m_healthManager)
			m_healthManager.OnLifeReachZero -= Defeat;
	}
	
	// Update is called once per frame
	void Update () 
	{
		distanceFromTarget = Vector3.Distance(transform.position, m_Target.position);
		m_TargetGlobalPos = transform.InverseTransformDirection(m_Target.localPosition - transform.localPosition);

		CalculateCanonsRotation(m_canonsManager.m_canonsRight, m_visor, m_rotationOffset);
	}

	private void CalculateCanonsRotation(Transform side, Transform visor, float offset){
		visor.LookAt(m_Target, transform.up);

		//HORIZONTAL ROTATION
		//Middle zone
		if(m_TargetGlobalPos.z < 29 && m_TargetGlobalPos.z > -9 && m_TargetGlobalPos.x < 150){
			//print("Zone R");
			m_canonsManager.SetAngleCanonRIGHT(side, 0 + offset);
			PlayerInRange = true;
		}
		//Upward/downward zone
		else{
			float angleRotRight = visor.localRotation.eulerAngles.y;
			angleRotRight = (angleRotRight > 180) ? angleRotRight - 360 : angleRotRight;
			//print((int)angleRight);
			//print((int)visor.localRotation.eulerAngles.y + "    :     " + (int)visor.eulerAngles.y);

			if(angleRotRight < RangeRotationRight && angleRotRight > -RangeRotationRight){
				m_canonsManager.SetAngleCanonRIGHT(side, angleRotRight + offset);
				PlayerInRange = true;
			}
			//Out range
			else{
				m_canonsManager.SetAngleCanonRIGHT(side, 0);
				PlayerInRange = false;
			}
		}

		//VERTICAL ROTATION
		float angleRotUp = Mathf.Exp(distanceFromTarget/RangeFactor);

		angleRotUp = (angleRotUp > 40) ? 40 : angleRotUp;
		angleRotUp = (angleRotUp < 0) ? 0 : angleRotUp;
		//print((int)angleRotUp);

		m_canonsManager.SetAngleCanonUP(side, (int)angleRotUp);

		//Shoot
		if(PlayerInRange && canShoot){
			//Debug.DrawLine(transform.position, m_Target.position, Color.red);
			m_canonsManager.ShootCanon(side);
			m_canonsManager.ReloadCanon(side);
		}
	}

	public void Defeat(object sender){

		m_healthManager.m_lifebar.bar.gameObject.SetActive(false);
		m_skull.SetActive(true);
		canShoot = false;
		imageUI.SetActive(false);
		m_mortar.canShoot = false;
	}
}
