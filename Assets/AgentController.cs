using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour {

	public float m_steps = 2;
	public ActionsController m_actions;
	private NavMeshAgent m_agent;
	//private float m_inputH;
	//private float m_inputV;
    private Transform m_Cam;
    private Vector3 m_CamForward;
	[HideInInspector] public Vector3 m_Move;

	private Vector3 Target;
	private bool isMoving;
	private bool canMove;

	// Use this for initialization
	void Start () {
		m_agent = GetComponent<NavMeshAgent>();

		if (Camera.main != null)
            m_Cam = Camera.main.transform;
        else
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//transform.rotation = Quaternion.Euler(0, 0, 0);        
		
		// calculate move direction to pass to character
        if (m_Cam != null)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = m_actions.m_inputV * m_CamForward * Time.deltaTime + m_actions.m_inputH * m_Cam.right * Time.deltaTime;
			Debug.DrawRay(transform.position, m_Move.normalized, Color.red);
        }
        else
            Debug.LogError("No main Camera");
		
        isMoving = (m_Move.magnitude > 0);


		//Target = transform.position + m_Move * 100;
		//if(isMoving){
			//m_agent.SetDestination(Target);
		//}
		//m_agent.Move(m_Move.normalized*m_steps);
		m_agent.velocity = m_Move.normalized*m_steps;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		//Gizmos.DrawWireSphere(Target, 0.5f);
	}
}
