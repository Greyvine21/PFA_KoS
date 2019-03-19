using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetIA : MonoBehaviour {

	public 

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Gunner"){
			print("Gunner");
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, 0.5f);
	}
}
