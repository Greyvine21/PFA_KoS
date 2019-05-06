using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController_Receiver : MonoBehaviour {

	public AgentController m_ControllerAgent;

	private FloatingShip m_ship;

	// Use this for initialization
	void Start () 
	{
		m_ship = GetComponentInParent<FloatingShip>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.localPosition = m_ControllerAgent.transform.localPosition;
		transform.localRotation = m_ControllerAgent.transform.localRotation;
	}
}
