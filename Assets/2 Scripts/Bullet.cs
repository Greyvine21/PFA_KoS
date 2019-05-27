using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	//public bool showHit;
	public float m_livingTime = 10;
	public int m_damage = 5;
	public List<string>  m_targetTag;
	
	void Start () 
	{
		Destroy(gameObject, m_livingTime);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponentInParent<EnnemyShipBehaviour>() !=  null){
			//print("hit : " + other.name);
			if(m_targetTag.Contains(other.GetComponentInParent<EnnemyShipBehaviour>().gameObject.tag)){
				switch (other.tag)
				{
					case "HullSystem":
						other.GetComponentInParent<healthManager>().DecreaseLife(0, m_damage);
					break;
					case "SailSystem":
						other.GetComponentInParent<healthManager>().DecreaseLife(0, m_damage);
						other.GetComponentInParent<healthManager>().DecreaseLife(1, m_damage);
					break;
					case "NavigationSystem":
						other.GetComponentInParent<healthManager>().DecreaseLife(0, m_damage);
						other.GetComponentInParent<healthManager>().DecreaseLife(2, m_damage);
					break;
					case "BridgeSystem":
						other.GetComponentInParent<healthManager>().DecreaseLife(0, m_damage);
						other.GetComponentInParent<healthManager>().DecreaseLife(3, m_damage);
					break;
					default:					
					break;
				}
			}
		}

		Destroy(gameObject);
	}
}
