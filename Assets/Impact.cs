using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Impact : MonoBehaviour
{
	public bool isActive;
	public ImpactZone m_parentZone;

	private bool canBeActivated = true;
	private Collider m_impactCollider;
	private MeshRenderer m_mesh;

	private healthManager m_health;

	void Awake()
	{
		m_mesh = GetComponent<MeshRenderer>();
		m_impactCollider = GetComponent<Collider>();
		m_health = GetComponentInParent<healthManager>();
	}

	public bool ActiveImpact(){
		if(canBeActivated){
			//gameObject.SetActive(true);
			m_mesh.enabled = true;
			m_impactCollider.enabled = true;
			m_parentZone = GetComponentInParent<ImpactZone>();
			canBeActivated = false;
			isActive = true;
			name = "Impact OK";
			return true;
		}
		return false;
	}

	public void DisableImpact(bool cd = false){
		isActive = false;
		m_mesh.enabled = false;
		m_impactCollider.enabled = false;
		name = "Impact";
		if(cd)
			StartCoroutine("DisableCooldown");
	}

	private IEnumerator DisableCooldown(){

		yield return new WaitForSeconds(m_health.m_ImpactCooldown);

		canBeActivated = true;
	}
}
