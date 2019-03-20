using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public bool showHit;
	public float m_livingTime = 10;
	void Start () 
	{
		Destroy(gameObject, m_livingTime);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(showHit)
			print("hit : " +other.gameObject.name);
		Destroy(gameObject);
	}
}
