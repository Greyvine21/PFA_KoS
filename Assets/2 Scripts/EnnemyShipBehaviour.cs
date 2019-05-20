using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyShipBehaviour : FloatingShip {

	[Header("IA movement")]
	public Transform m_navmeshBoat;
	public Transform m_lookAgent;
	public LayerMask m_raycastMask;
	public float m_raycastRange;

	[Header("IA Shoot")]
	public bool canShoot;
	public float m_aggroDistance = 200;
	public float m_rotationOffset = 10;
	public float RangeFactor = 40.5f;
	public Transform m_visorLeft;
	public Transform m_visorRight;

	[Header("IA monitoring")]
	public bool isInCombat;
	public bool PlayerInRange;
	public bool ObstacleDetected;
	public float distanceFromTarget;
	public float distanceFromAgent;
	public float angleFromAgent;

	private float DistFromObstacle;
	private CanonManager m_canonsManager;


	//Target Info
	private Transform m_Target;
	private Vector3 m_TargetGlobalPos;
	private Vector3 m_TargetDirection;
	private FloatingShip m_TargetShip;

	new void Start () 
	{
		base.Start();
		//
		m_canonsManager = GetComponent<CanonManager>();
		//
		m_Target = GameObject.FindGameObjectWithTag("PlayerShip").transform;
		m_TargetShip = m_Target.GetComponent<FloatingShip>();
        
		
		m_canonsManager.SetAngleCanonUP(m_canonsManager.m_canonsLeft, 20);
		m_canonsManager.SetAngleCanonUP(m_canonsManager.m_canonsRight, 20);
	}
	

	void Update () 
	{
		// Find Target
		if(m_Target){
			UpdateTargetPositionAndDirection();
		}
		else{
			m_Target = GameObject.FindGameObjectWithTag("PlayerShip").transform;
		}

		//
		FollowAgent();
		ObstacleDetector();
		SailsManagerIA();
		SailsStateUpdate();

		//Movement
		//transform.localPosition = new Vector3(m_navmeshBoat.localPosition.x, transform.localPosition.y, m_navmeshBoat.localPosition.z);

		//Combat
		if(isInCombat){
						
			if(distanceFromTarget > m_aggroDistance + 100){
				//print(name + " is out combat");
				isInCombat = false;
			}
			SelectCanonsSide();
		}
		else{

			if(distanceFromTarget < m_aggroDistance){
				//print(name + " is in combat");
				isInCombat = true;
			}
		}
	}

	void FixedUpdate()
	{
		Float();
			
		AddMainForce(forward);
		TurningForce();
		//m_shipRB.velocity = m_TargetDirection;
	}

	private void ObstacleDetector(){
		RaycastHit hit;
		Vector3 start = transform.position + transform.forward*30;
		float radius = 15;

		if(Physics.SphereCast(start, radius, transform.forward, out hit, m_raycastRange, m_raycastMask, QueryTriggerInteraction.Ignore)){
			if(hit.collider.gameObject.layer != 15){
				//print(hit.collider.gameObject.name + " has been hit");
				if(!ObstacleDetected) ObstacleDetected = true;
				DistFromObstacle = Vector3.Distance(transform.position, hit.point);
			}
			else{
				//print(hit.collider.gameObject.name + " has been hit");
			}

		}else{
			if(ObstacleDetected) ObstacleDetected = false;
		}
		
		//Debug.DrawRay(start + transform.right*radius, transform.forward * m_raycastRange, Color.red);
		//Debug.DrawRay(start - transform.right*radius, transform.forward * m_raycastRange, Color.red);
	}

	private void FollowAgent(){
		RaycastHit hit;
		Vector3 start = transform.position + transform.forward*30;
		//float radius = 15;
		//distanceFromAgent = Vector3.Distance(transform.position, m_navmeshBoat.position);
		if(Physics.Raycast(start, transform.forward, out hit, Mathf.Infinity, 17, QueryTriggerInteraction.Collide)){

			//print(hit.collider.gameObject.name + " has been hit");
			//Debug.DrawLine(start, hit.point, Color.red);
			SetRotationRudder(0);

		}else{

			//Debug.DrawRay(start, transform.forward * 2000, Color.red);
			SetRotationRudder((transform.InverseTransformDirection(m_navmeshBoat.localPosition - transform.localPosition).x > 0)? -30 : 30);

			m_lookAgent.LookAt(m_navmeshBoat.position, transform.up);
			angleFromAgent = Mathf.Abs(Vector3.Angle(transform.forward, m_lookAgent.forward));

			// Vector3 GlobalPos = transform.InverseTransformDirection(m_navmeshBoat.localPosition - transform.localPosition);

			// if(GlobalPos.x < 0) //Left
			// 	SetRotationRudder(15);
			// else if(GlobalPos.x < 0) //right
			// 	SetRotationRudder(-15);
		}
		
	}

	private void SailsManagerIA(){		
		if(ObstacleDetected){
			if(DistFromObstacle  < m_raycastRange/2){
				if(m_sailsSate != SailsState.sailsZeroPerCent)
					m_sailsSate = SailsState.sailsZeroPerCent;
			}
			else{
				if(m_sailsSate != SailsState.sailsFiftyPerCent)
					m_sailsSate = SailsState.sailsFiftyPerCent;
			}
		}
		else{
			if(angleFromAgent > 30){
				if(m_sailsSate != SailsState.sailsZeroPerCent)
					m_sailsSate = SailsState.sailsZeroPerCent;
			}
			else if(angleFromAgent > 20){ 
				if(m_sailsSate != SailsState.sailsFiftyPerCent)
					m_sailsSate = SailsState.sailsFiftyPerCent;
			}
			else{
				if(m_sailsSate != SailsState.sailsHundredPerCent)
					m_sailsSate = SailsState.sailsHundredPerCent;
			}
		}
	}

	private void UpdateTargetPositionAndDirection(){
		//Script
		if(!m_TargetShip)
			m_TargetShip = m_Target.GetComponent<FloatingShip>();
		//distance
		distanceFromTarget = Vector3.Distance(transform.position, m_Target.position);

		//Local position	
		m_TargetGlobalPos = transform.InverseTransformDirection(m_Target.localPosition - transform.localPosition);

		//Direction
		m_TargetDirection = m_TargetShip.m_shipVelocity;

		//DEBUG
		//Debug.DrawRay(m_TargetPos, m_TargetDirection*10, Color.red);
		//Debug.DrawLine(m_TargetPos - Vector3.right*10, m_TargetPos + Vector3.right*10, Color.red);
		//Debug.DrawLine(m_TargetPos - Vector3.forward*10, m_TargetPos + Vector3.forward*10, Color.red);
	}

	private void SelectCanonsSide(){

		//Player is right
		if(m_TargetGlobalPos.x > 0){
			CalculateCanonsRotation(m_canonsManager.m_canonsRight, m_visorRight, -m_rotationOffset);
		}
		//Player is left
		else if(m_TargetGlobalPos.x < 0){
			CalculateCanonsRotation(m_canonsManager.m_canonsLeft, m_visorLeft, m_rotationOffset);
		}
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

			if(angleRotRight< 30 && angleRotRight > -30){
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
			Debug.DrawLine(transform.position, m_Target.position, Color.red);
			m_canonsManager.ShootCanon(side);
			m_canonsManager.ReloadCanon(side);
		}
	}
}
