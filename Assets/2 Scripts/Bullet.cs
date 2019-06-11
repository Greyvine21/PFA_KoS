using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	//public bool showHit;
	public float m_livingTime = 10;
	public bool m_heal = false;
	public int m_value = 10;
	public GameObject m_impactFX_Water;
	public GameObject m_impactFX_Env;
	public GameObject m_impactFX_Wood;
	public List<string>  m_DamageTargetList;
	
	void Start () 
	{
		Destroy(gameObject, m_livingTime);
	}
	
	void OnTriggerEnter(Collider other)
	{
		//print(other.name);
		switch (other.tag)
		{
			case "Ocean":
				//print("hit water");
				if(m_impactFX_Water)
					Instantiate(m_impactFX_Water, transform.position, Quaternion.Euler(-90,0,0));
			break;
			case "Environment":
				//print("hit water");
				if(m_impactFX_Env)
					Instantiate(m_impactFX_Env, transform.position, Quaternion.Euler(0,0,0));
			break;
			default:
				if(other.gameObject.layer != gameObject.layer){
					if(m_impactFX_Wood)
						Instantiate(m_impactFX_Wood, transform.position,Quaternion.Euler(-90,0,0));
					Damage(other.tag, other.GetComponentInParent<healthManager>());
				}		
				/*if(other.GetComponentInParent<EnnemyShipBehaviour>() !=  null){
					//print("hit Enemy : " + other.name);
					if(m_DamageTargetList.Contains(other.GetComponentInParent<EnnemyShipBehaviour>().gameObject.tag)){
						if(m_impactFX_Wood)
							Instantiate(m_impactFX_Wood, transform.position,Quaternion.Euler(-90,0,0));
						Damage(other.tag, other.GetComponentInParent<healthManager>());
					}
				}
				else if(other.GetComponentInParent<PlayerShipBehaviour>() !=  null){
					//print("hit Player : " + other.name);
					if(m_DamageTargetList.Contains(other.GetComponentInParent<PlayerShipBehaviour>().gameObject.tag)){
						if(m_impactFX_Wood)
							Instantiate(m_impactFX_Wood, transform.position,Quaternion.Euler(-90,0,0));
						Damage(other.tag, other.GetComponentInParent<healthManager>());
					}
				}*/
			break;
		}

		//Destroy(gameObject);
	}

	private void Damage(string tag, healthManager health){
		
		if(m_heal)
			health.IncreaseLife(m_value);
		else
			health.DecreaseLife(m_value);
		//
		if(health.m_useImpact){
			switch (tag)
			{
				case "HullSystem":
				break;
				case "SailSystem":
					health.SpawnImpact(health.m_impactSails);
				break;
				case "NavigationSystem":
					health.SpawnImpact(health.m_impactNavigation);
				break;
				case "BridgeSystem":
					health.SpawnImpact(health.m_impactBridge);
				break;
				default:					
				break;
			}
		}	
	}
}
