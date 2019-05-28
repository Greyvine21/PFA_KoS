using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum State
{
	Patrol,
	Chase,
	Flee, 
	Combat
}

public class BoatAgent : MonoBehaviour {

	public EnnemyShipBehaviour m_EnnemyShip;
	public Transform m_targetTemp;

	[Header("State Machine")]
	public bool m_useStateMachine;
	public State m_currentState = State.Patrol;

	[Header("Patrol State")]
	[SerializeField] private PatrolPath m_patrolPath = null;

	[Header("Chase State")]
	public float m_chaseDistance = 200;
	public Vector3 m_offsetChase;

	[Header("Combat State")]
	public float m_checkObstacleNavmesh = 100;
	public float m_combatDistance = 100;
	//public float m_combatRaycast;
	//public LayerMask m_combatRaycastMask;

	[Header("Monitoring value")]
	public bool PlayerDetected;
	public bool m_isCheckingNavmesh;
	public float m_distanceFromTargetPlayer;
	public float m_distanceFromEnnemyShip;
	public float m_agentVelocity;
	public float m_ennemyShipVelocity;
	public float m_TargetPlayerVelocity;

	//PRIVATE
	private NavMeshAgent m_agent;
	private PlayerShipBehaviour m_targetPlayer;
	private int m_currentPathIndex = 0;
	private Vector3 m_TargetPlayerDestination;
	//private Vector3 m_combatDestination = Vector3.zero;


	#region MonoBehaviour
	void Start () 
	{
		m_agent = GetComponent<NavMeshAgent>();
		//
		m_targetPlayer = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<PlayerShipBehaviour>();
	}

	void OnEnable()
	{
		m_currentPathIndex = 0;
	}

	void Update () 
	{
		//Get distances and Velocity
		m_distanceFromTargetPlayer = Vector3.Distance(transform.position, m_TargetPlayerDestination);
		m_distanceFromEnnemyShip = Vector3.Distance(transform.position, m_EnnemyShip.transform.position);
		m_agentVelocity = m_agent.velocity.magnitude;
		m_ennemyShipVelocity = m_EnnemyShip.m_shipVelocity.magnitude;
		m_TargetPlayerVelocity = m_targetPlayer.m_shipRB.velocity.magnitude;
		
		//
		DetectTarget();
		
		//Move
		if(m_useStateMachine){
			//Update the state		
			StateControllerUpdate();
		}
		else{
			m_agent.SetDestination(m_targetTemp.position);
		}
		
		//if(m_currentState != State.Combat){
			//Clamp agent velocity with ship velocity
			if(m_distanceFromEnnemyShip < 60){
				m_agent.speed = m_ennemyShipVelocity*2;
			}else{
				m_agent.speed = m_ennemyShipVelocity;
			}

			//Stop the agent if the boat is too far
			if(m_distanceFromEnnemyShip > 70){
				if(!m_agent.isStopped) m_agent.isStopped = true;
			}else{
				if(m_agent.isStopped) m_agent.isStopped = false;
			}
		//}

	}
	#endregion MonoBehaviour

	#region StateMachine
	private void StateControllerUpdate()
	{
		switch (m_currentState)
		{
			case State.Patrol:
			{
				if(PlayerDetected){
					if(m_agent.hasPath) m_agent.ResetPath();
					ChangeState(State.Chase);
				}
				else{
					Patrol();
				}
				break;
			}
			case State.Chase:
			{				
				if(!PlayerDetected){
					if(m_agent.hasPath) m_agent.ResetPath();
					ChangeState(State.Patrol);
				}
				else if(m_distanceFromTargetPlayer < 5f && m_EnnemyShip.distanceFromTarget < m_combatDistance){
					if(m_agent.hasPath) m_agent.ResetPath();
					ChangeState(State.Combat);
				}
				else{
					Chase();
				}
				break;
			}
			// case State.Flee:
			// {
			// 	break;
			// }
			case State.Combat:
			{	
				if(!PlayerDetected){
					if(m_agent.hasPath) m_agent.ResetPath();
					ChangeState(State.Patrol);
				}
				else if(m_EnnemyShip.distanceFromTarget > m_combatDistance){
					if(m_agent.hasPath) m_agent.ResetPath();
					ChangeState(State.Chase);
				}
				else{
					Combat();
				}
				break;
			}
			default: 
			break;
		}
	}

	private void ChangeState(State nextState)
	{
		m_currentState = nextState;
		StateControllerUpdate();
	}
	#endregion StateMachine

	#region States
	private void Combat(){

		Vector3 nextPos = transform.position + m_targetPlayer.m_shipRB.velocity.normalized * m_checkObstacleNavmesh;
		nextPos = new Vector3(nextPos.x, -6, nextPos.z);

		m_agent.autoBraking = false;
		NavMeshHit hit;
		if (!NavMesh.SamplePosition(nextPos, out hit, 1.0f, NavMesh.AllAreas))
        {
			//print("Not On navmesh");
			m_isCheckingNavmesh = true;
			m_checkObstacleNavmesh += 20;
			//nextPos += transform.position + m_targetPlayer.m_shipRB.velocity.normalized*20;
    	}
		else
		{
			//print("OK");
			m_isCheckingNavmesh = false;
			m_checkObstacleNavmesh = 100;
			m_agent.SetDestination(nextPos);
		}
		
		Debug.DrawLine(nextPos - Vector3.right*5, nextPos + Vector3.right*5, Color.blue);
		Debug.DrawLine(nextPos - Vector3.forward*5, nextPos + Vector3.forward*5, Color.blue);

		//CalulateTargetDestination();
		//transform.position = m_TargetPlayerDestination;
	}

	private void Chase(){

		CalulateTargetDestination();

		m_agent.SetDestination(m_TargetPlayerDestination);
	}

	private void Patrol(){
		if (m_agent.hasPath == true && m_agent.remainingDistance > m_agent.stoppingDistance)
		{
			return;
		}

		Vector3 targetDestination = Vector3.zero;
		NavMeshPath navMeshPath = new NavMeshPath();
		m_agent.autoBraking = true;

		if (m_patrolPath == null)
		{
			Debug.LogError("No path found");
		}
		else
		{
			if (m_currentPathIndex >= m_patrolPath.Nodes.Count)
			{
				m_currentPathIndex = 0;
			}

			targetDestination = m_patrolPath.Nodes[m_currentPathIndex].position;
			m_currentPathIndex++;
		}
		//print(targetDestination);
		if (targetDestination != Vector3.zero && m_agent.CalculatePath(targetDestination, navMeshPath))
		{
			//print("path ok");
			m_agent.SetPath(navMeshPath);
		}
	}
	#endregion States

	#region others
	private void CalulateTargetDestination(){
		// Detecter si Objet2 est à droite ou à gauche par rapport au forward d'Objet1
		// Vector3 GlobalPos = Objet1.InverseTransformDirection(Objet2.localPosition - Objet1.localPosition);
		// Si GlobalPos.x < 0 = Objet2 est à gauche d'Objet1
		// Si GlobalPos.x > 0 = Objet2 est à droite d'Objet1
		
		Vector3 m_targetGlobalPos = m_targetPlayer.transform.InverseTransformDirection(m_EnnemyShip.transform.localPosition - m_targetPlayer.transform.localPosition);
		//print(m_targetGlobalPos.x);

		Vector3 offset = (m_targetGlobalPos.x < 0) ? new Vector3(-m_offsetChase.x,m_offsetChase.y,m_offsetChase.z) : new Vector3(m_offsetChase.x,m_offsetChase.y,m_offsetChase.z);
		
		m_TargetPlayerDestination = m_targetPlayer.transform.TransformPoint(offset);
		m_TargetPlayerDestination = new Vector3(m_TargetPlayerDestination.x, -6, m_TargetPlayerDestination.z);
		
		NavMeshHit hit;
		if (!NavMesh.SamplePosition(m_TargetPlayerDestination, out hit, 1.0f, NavMesh.AllAreas))
        {
			offset = new Vector3(-offset.x,offset.y,offset.z);
			m_TargetPlayerDestination = m_targetPlayer.transform.TransformPoint(offset);
    	}
		m_TargetPlayerDestination = new Vector3(m_TargetPlayerDestination.x, -6, m_TargetPlayerDestination.z);

		//
		Debug.DrawLine(m_TargetPlayerDestination - Vector3.right*5, m_TargetPlayerDestination + Vector3.right*5, Color.yellow);
		Debug.DrawLine(m_TargetPlayerDestination - Vector3.forward*5, m_TargetPlayerDestination + Vector3.forward*5, Color.yellow);
		//
	}
	
	private void DetectTarget(){
		if(PlayerDetected){		
			if(m_EnnemyShip.distanceFromTarget > m_chaseDistance){
				//print(name + " is out combat");
				m_chaseDistance /= 1.2f;
				PlayerDetected = false;
			}
		}
		else{
			if(m_EnnemyShip.distanceFromTarget < m_chaseDistance){
				//print(name + " is in combat");
				m_chaseDistance *= 1.2f;
				PlayerDetected = true;
			}
		}
	}
	#endregion others


	void OnDrawGizmos()
	{
		switch (m_currentState)
		{
			case State.Patrol:
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(m_EnnemyShip.transform.position, m_chaseDistance);
				break;
			}
			case State.Chase:
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(m_TargetPlayerDestination - Vector3.right*5, m_TargetPlayerDestination + Vector3.right*5);
				Gizmos.DrawLine(m_TargetPlayerDestination - Vector3.forward*5, m_TargetPlayerDestination + Vector3.forward*5);
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(m_EnnemyShip.transform.position, m_combatDistance);
				break;
			}
			case State.Flee:
			{
				break;
			}
			case State.Combat:
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(m_EnnemyShip.transform.position, m_combatDistance);
				//Gizmos.DrawLine(transform.position, m_combatDestination);
				//Gizmos.DrawWireSphere(m_combatDestination, 10);
				//Gizmos.color = Color.green;
				//Gizmos.DrawRay(transform.position, m_agent.velocity*10);
				break;
			}
			default: 
			break;
		}
	}
}
