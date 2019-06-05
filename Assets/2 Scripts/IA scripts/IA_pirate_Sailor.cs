using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_pirate_Sailor : IA_pirate {

	public bool repair = false;

	public void GoTo(Vector3 pos){
		canWander = false;
		m_agent.stoppingDistance = 2;
		NavMeshHit navHit;
 
        if(NavMesh.SamplePosition(pos, out navHit, 3, -1)){
			m_agent.SetDestination(navHit.position);
		}
	}
}
