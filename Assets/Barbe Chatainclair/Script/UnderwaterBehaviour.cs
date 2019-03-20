using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterBehaviour : MonoBehaviour {

	private Ocean ocean;
	public float waterLevel; 
	public bool isUnderwater;

	public Color normalColor;
	public float normalDensity = 0.001f;
	public Color underwaterColor;
	public float underwaterDensity = 0.03f;

	public float waterHeight;

	void OnEnable ()
	{
		if (ocean == null)
			ocean = GameObject.FindGameObjectWithTag ("Ocean").GetComponent<Ocean>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		waterHeight = ocean.GetWaterHeightAtLocation(transform.position.x, transform.position.y);
		isUnderwater = transform.position.y < waterHeight;

		if(isUnderwater)
			SetUnderwater();
		else
			SetNormal();
	}

	private void SetUnderwater(){
		RenderSettings.fogColor = underwaterColor;
		RenderSettings.fogDensity = underwaterDensity;
	}

	private void SetNormal(){
		RenderSettings.fogColor = normalColor;
		RenderSettings.fogDensity = normalDensity;
	}
}
