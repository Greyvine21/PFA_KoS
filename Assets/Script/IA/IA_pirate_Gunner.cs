using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_pirate_Gunner : IA_pirate {
	
	[Header("Cannon")]
	public Transform m_leftCannon;
	public Transform m_rightCannon;
	public float m_cannonReachSpeedMultiplier = 3;

	float dist;

	//private Transform TargetTemp;

    private new void Update()
    {
        base.Update();

		if(!canWander){
			if(!wanderCoroutine && !m_agent.pathPending){
				if (m_agent.remainingDistance <= m_agent.stoppingDistance)
				{
					if (!m_agent.hasPath || m_agent.velocity.sqrMagnitude == 0f)
					{
						print("has arrived");
						//transform.LookAt(TargetTemp.parent, transform.parent.up);
						StartCoroutine(StopWander());
					}
				}
			}
		}
	}

	public void MoveToleftCannon(){
		canWander = false;
		m_agent.speed = m_agent.speed*m_cannonReachSpeedMultiplier;
		m_agent.enabled = true;
		m_agent.SetDestination(m_leftCannon.position);
		//TargetTemp = m_leftCannon;
	}

	public void MoveTorightCannon(){
		canWander = false;
		m_agent.speed = m_agent.speed*m_cannonReachSpeedMultiplier;
		m_agent.enabled = true;
		m_agent.SetDestination(m_rightCannon.position);
		//TargetTemp = m_rightCannon;
	}

	public void SwitchCannonBall(){

	}

}
