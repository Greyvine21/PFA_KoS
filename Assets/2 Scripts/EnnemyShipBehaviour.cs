using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyShipBehaviour : FloatingShip {

	[Header("IA movement")]
	public bool canMove;
	public Transform m_navmeshBoat;
	public Transform m_lookAgent;
	public LayerMask m_raycastMask;
	public float m_raycastRange;

	[Header("IA Shoot")]
	public bool canShoot;
	public float m_rotationOffset = 10;
	public float RangeFactor = 40.5f;
	public Transform m_visorLeft;
	public Transform m_visorRight;

	[Header("IA monitoring")]
	public bool isDefeated;
	public bool PlayerInRange;
	public bool ObstacleDetected;
	public float distanceFromTarget;
	public float angleFromAgent;

	//PRIVATE
	private float DistFromObstacle;
	private CanonManager m_canonsManager;

	//Target Info
	private Transform m_Target;
	private Vector3 m_TargetGlobalPos;
	//private Vector3 m_TargetDirection;
	private PlayerShipBehaviour m_TargetShip;
	private BoatAgent m_boatAgent;

	new void Start () 
	{
		base.Start();
		//
		m_canonsManager = GetComponent<CanonManager>();
		//
		m_Target = GameObject.FindGameObjectWithTag("PlayerShip").transform;
		m_TargetShip = m_Target.GetComponent<PlayerShipBehaviour>();
		//
		m_boatAgent = m_navmeshBoat.GetComponent<BoatAgent>();
        
		m_canonsManager.SetAngleCanonUP(m_canonsManager.m_canonsLeft, 20);
		m_canonsManager.SetAngleCanonUP(m_canonsManager.m_canonsRight, 20);

		UpdateTargetPositionAndDirection();
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
		//
		

		//if(m_boatAgent.PlayerDetected)
			SelectCanonsSide();
	}

	void FixedUpdate()
	{
		Float();
		
		if(canMove){
			AddMainForce(forward);
			TurningForce();
		}
	}

	// private void DetectTarget(){
	// 	if(PlayerDetected){		
	// 		if(distanceFromTarget > m_chaseDistance){
	// 			//print(name + " is out combat");
	// 			m_chaseDistance /= 2f;
	// 			PlayerDetected = false;
	// 		}
	// 	}
	// 	else{
	// 		if(distanceFromTarget < m_chaseDistance){
	// 			//print(name + " is in combat");
	// 			m_chaseDistance *= 2f;
	// 			PlayerDetected = true;
	// 		}
	// 	}
	// }

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
			// else{
			// 	print(hit.collider.gameObject.name + " has been hit");
			// }
		}else{
			if(ObstacleDetected) ObstacleDetected = false;
		}
		
		//Debug.DrawRay(start + transform.right*radius, transform.forward * m_raycastRange, Color.red);
		//Debug.DrawRay(start - transform.right*radius, transform.forward * m_raycastRange, Color.red);
	}

	private void FollowAgent(){
		RaycastHit hit;
		Vector3 start = transform.position/* + transform.forward*30*/;
		//float radius = 15;
		//distanceFromAgent = Vector3.Distance(transform.position, m_navmeshBoat.position);
		if(Physics.Raycast(start, transform.forward, out hit, Mathf.Infinity, 17, QueryTriggerInteraction.Collide)){
			SetRotationRudder(0);
		}
		else
		{
			m_lookAgent.LookAt(m_navmeshBoat.position, transform.up);
			angleFromAgent = Mathf.Abs(Vector3.Angle(transform.forward, m_lookAgent.forward));

			//Debug.DrawRay(start, transform.forward * 2000, Color.red);
			if(angleFromAgent > 10)
				SetRotationRudder((transform.InverseTransformDirection(m_navmeshBoat.localPosition - transform.localPosition).x > 0)? -30 : 30);
			else
				SetRotationRudder(0);
		}
		
	}

	private void SailsManagerIA(){
		if(canMove){
			if(ObstacleDetected){
				if(DistFromObstacle  < m_raycastRange/2){
					m_externalSpeedMultiplier = 1f;
					if(m_sailsSate > SailsState.sailsZeroPerCent){
						//print("0 % : Obstacle");
						m_sailsSate = SailsState.sailsZeroPerCent;
					}
				}
				else{
					m_externalSpeedMultiplier = 1f;
					if(m_sailsSate > SailsState.sailsFiftyPerCent){
						//print("50 % : Obstacle");
						m_sailsSate = SailsState.sailsFiftyPerCent;
					}
				}
			}
			else{
				if(angleFromAgent > 30){
					m_externalSpeedMultiplier = 1f;
					if(m_sailsSate != SailsState.sailsZeroPerCent){
						//print("0 % : Rotate");
						m_sailsSate = SailsState.sailsZeroPerCent;
					}
				}
				else if(angleFromAgent > 20){
					m_externalSpeedMultiplier = 1f; 
					if(m_sailsSate != SailsState.sailsFiftyPerCent){
						//print("50 % : Rotate");
						m_sailsSate = SailsState.sailsFiftyPerCent;
					}
				}
				else{
					if(m_boatAgent.m_currentState == State.Combat){

						/*if(m_boatAgent.m_distanceFromEnnemyShip > 70)
							m_externalSpeedMultiplier = 1.5f;
						else if(m_boatAgent.m_distanceFromEnnemyShip < 60)
							m_externalSpeedMultiplier = 0.75f;
						else
							m_externalSpeedMultiplier = 1f;*/

						if(m_sailsSate != m_TargetShip.m_sailsSate){
							//print("copy target");
							m_sailsSate = m_TargetShip.m_sailsSate;
						}
					}else{
						m_externalSpeedMultiplier = 1f;	
						if(m_sailsSate != SailsState.sailsHundredPerCent){
							//print("100 %");
							m_sailsSate = SailsState.sailsHundredPerCent;
						}
					}
				}
			}
		}
		else{
			if(m_sailsSate > SailsState.sailsZeroPerCent){
				//print("0 % : Stop");
				m_sailsSate = SailsState.sailsZeroPerCent;
			}
		}
	}

	private void UpdateTargetPositionAndDirection(){
		//Script
		if(!m_TargetShip)
			m_TargetShip = m_Target.GetComponent<PlayerShipBehaviour>();
		
		//distance
		distanceFromTarget = Vector3.Distance(transform.position, m_Target.position);

		//Local position	
		m_TargetGlobalPos = transform.InverseTransformDirection(m_Target.localPosition - transform.localPosition);

		//Direction
		//m_TargetDirection = m_TargetShip.m_shipVelocity;

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

			if(angleRotRight < 30 && angleRotRight > -30){
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

	public void Defeat(){
		if(!isDefeated){
			isDefeated = true;
			canMove = false;
			canShoot = false;
			StartCoroutine("Sink");
		}
	}

	// new void OnDrawGizmos()
	// {
	// 	base.OnDrawGizmos();

	// 	Gizmos.color = Color.yellow;
	// 	Gizmos.DrawWireSphere(transform.position, m_aggroDistance);
	// }
}
