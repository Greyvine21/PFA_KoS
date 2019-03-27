using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SplineWalkerMode {
	Once,
	Loop,
	PingPong
}

public class CurveWalker : MonoBehaviour {

	public Curve m_curve;
	public float m_duration;
	public SplineWalkerMode mode;
	private float progress;

	private bool goingForward = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(goingForward){
			progress += Time.deltaTime / m_duration;

			if(progress > 1){
				switch (mode)
				{
					case SplineWalkerMode.Loop:
						progress -= 1f;
					break;
					case SplineWalkerMode.Once:
						progress = 1f;
					break;
					case SplineWalkerMode.PingPong:
						progress = 2f - progress;
						goingForward = false;
					break;
					default:
						progress = 1f;
					break;
				}
			}
		}
		else{
			progress -= Time.deltaTime / m_duration;
			if(progress < 0f){
				progress = -progress;
				goingForward = true;
			}
		}
		
		transform.localPosition = m_curve.GetPoint(progress);
	}
}
