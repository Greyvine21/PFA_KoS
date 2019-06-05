using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum Roles{
	Gunner,
	Sailor,
	Buccaneer,
	Ennemy
}

public class IA_pirate : MonoBehaviour {

	public Roles m_Role;

	[Header("Wandering")]
	public float wanderRadius = 1;
    public float wanderTimerMin = 0.2f;
    public float wanderTimerMax = 3f;
    private float wanderTimer;
    public float stopWanderTime = 5;
 
    private Transform target;
    private float timer;
	
	
	protected NavMeshAgent m_agent;
	protected bool wanderCoroutine = false;
	protected float m_baseSpeed;

	protected void Start () {
		m_agent = GetComponent<NavMeshAgent>();

		m_baseSpeed = m_agent.speed;
		wanderTimer = Random.Range(wanderTimerMin,wanderTimerMax);
        timer = wanderTimer;
	}
 
    protected void Wander(){
        timer += Time.deltaTime;

        if (timer >= wanderTimer) {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
			m_agent.enabled = true;
            m_agent.SetDestination(newPos);
			
			wanderTimer = Random.Range(wanderTimerMin,wanderTimerMax);
            timer = 0;
        }
    }

	public void GoTo(Vector3 pos, float stoppingDist = 2, float distanceOffset = 3){
		m_agent.stoppingDistance = stoppingDist;
		NavMeshHit navHit;
 
        if(NavMesh.SamplePosition(pos, out navHit, distanceOffset, -1)){
			m_agent.SetDestination(navHit.position);
		}
	}

    private static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }
}
