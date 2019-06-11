using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_pirate_Gunner : IA_pirate {
	
	
	[Header("Gunner")]
	public float m_minDist;
	private Vector3 m_startPos;
	private Quaternion m_startRot;
	new void Start()
	{
		base.Start();

		m_startPos = transform.position;
		m_startRot = transform.rotation;
	}

	void Update()
	{
		if(Vector3.Distance(transform.position, m_startPos) > m_minDist){
			m_agent.SetDestination(m_startPos);
		}
		else{
			if(transform.rotation != m_startRot)
				transform.rotation = m_startRot;
		}

	}

	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(m_startPos, 0.2f);
	}

}
