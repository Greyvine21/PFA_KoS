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
	public float m_travelTime;
	public SplineWalkerMode mode;
	private float progress;

	public bool launch = false;

	private bool goingForward = true;
	
	// Update is called once per frame
	void Update () {
		if(launch){
			if(goingForward){
				progress += Time.deltaTime / m_travelTime;

				if(progress > 1){
					switch (mode)
					{
						case SplineWalkerMode.Loop:
							progress -= 1f;
						break;
						case SplineWalkerMode.Once:
							Destroy(gameObject);
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
				progress -= Time.deltaTime / m_travelTime;
				if(progress < 0f){
					progress = -progress;
					goingForward = true;
				}
			}
			
			transform.position = (m_curve.GetLocalPoint(progress));
		}
	}
}
