using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController_Receiver : MonoBehaviour {

	public GameObject m_ControllerAgent;

	private FloatingShip m_ship;

	// Use this for initialization
	void Start () 
	{
		m_ship = GetComponentInParent<FloatingShip>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = m_ship.transform.position + m_ControllerAgent.transform.position;
		transform.rotation = m_ship.transform.rotation * m_ControllerAgent.transform.rotation;
	}
}
