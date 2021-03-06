﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController_Receiver : MonoBehaviour {

    [SerializeField] private Transform m_upperBody;
	public NavMeshAgent m_Agent;
	public bool m_randomIdleAnim;
	public float m_delayMin;
	public float m_delayMax;


	[Header("Monitoring")]
	public AgentController m_ControllerAgent;
	public Animator m_animator;


	// Use this for initialization
	void Start () 
	{
		m_ControllerAgent = m_Agent.GetComponent<AgentController>();

		if(m_animator && m_randomIdleAnim)
			StartCoroutine("TriggerRandomAnim");
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.localPosition = m_Agent.transform.localPosition;
		transform.localRotation = m_Agent.transform.localRotation;
		
		if(m_upperBody)
        	m_upperBody.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -transform.rotation.eulerAngles.z);

		if(m_animator){
			m_animator.SetFloat("Velocity" , m_Agent.velocity.magnitude);
		}		
	}

	private IEnumerator TriggerRandomAnim(){
		m_animator.SetFloat("randFloat" , 0);
		float delay = Random.Range(m_delayMin, m_delayMax);
		//print(delay);

		yield return new WaitForSeconds(delay);

		m_animator.SetFloat("randFloat" , Random.Range(1, 4));

		yield return new WaitForSeconds(1);
		StartCoroutine("TriggerRandomAnim");
	}
}
