using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
	
	void Update () 
	{
		transform.LookAt(Camera.main.transform);
		//transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y, transform.localScale.z);
	}
}
