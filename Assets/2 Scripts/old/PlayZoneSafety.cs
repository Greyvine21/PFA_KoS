using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayZoneSafety : MonoBehaviour {

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Player"){
			other.transform.position = Vector3.zero;
			print("player out");
		}
	}
}
