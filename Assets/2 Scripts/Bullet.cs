using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	//public bool showHit;
	public float m_livingTime = 10;
	public int m_damage = 5;
	public GameObject m_impactFX;
	public List<string>  m_targetTag;
	
	void Start () 
	{
		Destroy(gameObject, m_livingTime);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponentInParent<EnnemyShipBehaviour>() !=  null){
			//print("hit Enemy : " + other.name);
			//FX
			Instantiate(m_impactFX, transform.position, Quaternion.identity);
			//
			if(m_targetTag.Contains(other.GetComponentInParent<EnnemyShipBehaviour>().gameObject.tag)){
				Damage(other.tag, other.GetComponentInParent<healthManager>());
			}
		}
		else if(other.GetComponentInParent<PlayerShipBehaviour>() !=  null){
			//print("hit Player : " + other.name);
			//FX
			Instantiate(m_impactFX, transform.position, Quaternion.identity);
			//
			if(m_targetTag.Contains(other.GetComponentInParent<PlayerShipBehaviour>().gameObject.tag)){
				Damage(other.tag, other.GetComponentInParent<healthManager>());
			}
		}
		Destroy(gameObject);
	}

	private void Damage(string tag, healthManager health){
		
		health.DecreaseLife(0, m_damage); //hull				
		switch (tag)
		{
			case "HullSystem":
			break;
			case "SailSystem":
				health.SpawnImpact(health.m_impactSails);
				//health.DecreaseLife(1, m_damage); //Sails
			break;
			case "NavigationSystem":
				health.SpawnImpact(health.m_impactNavigation);
				//health.DecreaseLife(2, m_damage); //Nav
			break;
			case "BridgeSystem":
				health.SpawnImpact(health.m_impactBridge);
				//health.DecreaseLife(3, m_damage); //Bridge
			break;
			default:					
			break;
		}
	}
}
