using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {

	public bool followTarget;
	public Transform target;
	public float Y;

	private Quaternion startRot;
	// Use this for initialization
	void Start () {
		startRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(followTarget){
			transform.position = new Vector3(target.position.x, Y, target.position.z);
			transform.rotation = startRot;
		}
	}
}
