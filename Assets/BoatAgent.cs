using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoatAgent : MonoBehaviour {

	public float m_dist = 20;
	public float m_destinationThresholdMin = 6;
	public float m_destinationThresholdMax = 15;
	public bool m_followPlayer;
	public Transform m_targetTemp;
	public FloatingShip m_ship;

	[Header("Monitoring value")]
	public bool m_targetReachSide;
	public float m_distanceFromTarget;
	public float m_velocity;
	public float m_TargetPlayerVelocity;
	public float m_ennemyShipVelocity;
	private NavMeshAgent m_agent;
	private Transform m_target;
	private FloatingShip m_targetShip;

	private Vector3 m_destination;

	void Start () 
	{
		m_agent = GetComponent<NavMeshAgent>();
		//
		m_target = GameObject.FindGameObjectWithTag("PlayerShip").transform;
		m_targetShip = m_target.GetComponent<FloatingShip>();
	}
	

	void Update () 
	{
		m_velocity = m_agent.velocity.magnitude;
		m_TargetPlayerVelocity = m_targetShip.m_shipRB.velocity.magnitude;
		m_ennemyShipVelocity = m_ship.m_shipVelocity.magnitude;
		m_distanceFromTarget = Vector3.Distance(transform.position, m_destination);
		
		m_agent.speed = m_ennemyShipVelocity;

		if(m_followPlayer){
			/*
			Detecter si Objet2 est à droite ou à gauche par rapport au forward d'Objet1
			Vector3 GlobalPos = Objet1.InverseTransformDirection(Objet2.localPosition - Objet1.localPosition);
			Si GlobalPos.x < 0 = Objet2 est à gauche d'Objet1
			Si GlobalPos.x > 0 = Objet2 est à droite d'Objet1
			*/
			
			Vector3 m_targetGlobalPos = m_target.InverseTransformDirection(transform.localPosition - m_target.localPosition);
			//print(m_targetGlobalPos.x);

			//Debug.DrawLine(m_targetGlobalPos - Vector3.right*5, m_targetGlobalPos + Vector3.right*5, Color.yellow);
			//Debug.DrawLine(m_targetGlobalPos - Vector3.forward*5, m_targetGlobalPos + Vector3.forward*5, Color.yellow);

			Vector3 offset = (m_targetGlobalPos.x < 0) ? new Vector3(-m_dist,0,30) : new Vector3(m_dist,0,30);
			
			m_destination = m_target.TransformPoint(offset);
			//
			Debug.DrawLine(m_destination - Vector3.right*5, m_destination + Vector3.right*5, Color.blue);
			Debug.DrawLine(m_destination - Vector3.forward*5, m_destination + Vector3.forward*5, Color.blue);
			//
				
			// if(!m_targetReachSide){
			// 	if(m_distanceFromTarget < m_destinationThresholdMin){
			// 		m_targetReachSide = true;
			// 	}
			// }
			// else{
			// 	m_agent.velocity = m_targetShip.m_shipRB.velocity;
			// 	if(m_distanceFromTarget > m_destinationThresholdMax){
			// 		m_targetReachSide = false;
			// 	}
			// }
		}
		else{
			m_destination = m_targetTemp.position;
			//print(Vector3.Distance(transform.position, m_ship.transform.position));

		}

		
		m_agent.SetDestination(m_destination);

		if(Vector3.Distance(transform.position, m_ship.transform.position) > 70){
			if(!m_agent.isStopped) m_agent.isStopped = true;
		}else{
			if(m_agent.isStopped) m_agent.isStopped = false;
		}
	}
}
