using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPhysics : MonoBehaviour {

	[Header("Floating force")]	
	[SerializeField] private float m_floatingCoef;
	[SerializeField] private float m_maxFloating = 4f;
	[SerializeField] private ForceMode m_force;
	//[SerializeField] private float m_maxVelocity = 4f;
	[SerializeField] private Buoy[] m_buoy;
	[SerializeField] private Ocean m_ocean;

	[Header("Monitoring")]
	[SerializeField] private float m_waterLevel;
	[SerializeField] private Vector3 m_shipVelocity;
	private Rigidbody m_shipRB;

	
	
	void Start () {
		m_shipRB = GetComponent<Rigidbody>();
		
	}
	
	void FixedUpdate()
	{
		//float
		Float();
	}

	
	//FLOAT
	void ApplyFloatingForce(Buoy point, float coef)
	{
		Vector3 FloatingForce = point.direction * coef * m_floatingCoef;
		m_shipRB.AddForceAtPosition(FloatingForce, point.transform.position, m_force);
		Debug.DrawRay(point.transform.position, FloatingForce, Color.red,0.5f);
	}	
	
	void ApplyGravityForce(Buoy point, float coef)
	{
		Vector3 FloatingForce = -point.direction * coef * m_floatingCoef;
		m_shipRB.AddForceAtPosition(FloatingForce, point.transform.position, m_force);
		Debug.DrawRay(point.transform.position, FloatingForce, Color.yellow,0.5f);
	}
	void Float()
	{
		foreach (Buoy point in m_buoy)
		{
			//*
			//print(point.transform.name + " :  x: " + point.transform.position.x + "  y: " + point.transform.position.y);
			m_waterLevel = m_ocean.GetWaterHeightAtLocation(point.transform.position.x, point.transform.position.y);
			/*/
			m_waterLevel = 0;
			//m_waterLevel = WaterController.current.GetWaveYPos(point.position, Time.time);
			//*/

			if(point.transform.position.y < m_waterLevel){
				float diff = m_waterLevel - point.transform.position.y;
				//print(point.name + " diff = "+ diff);
				if(Mathf.Abs(diff) > m_maxFloating)
					ApplyFloatingForce(point, m_maxFloating);
				else
					ApplyFloatingForce(point, Mathf.Abs(diff));
			}
			else{
				if(point.applyGravityForce){
					float diff = point.transform.position.y - m_waterLevel;
					//print(point.name + " diff = "+ diff);
					if(Mathf.Abs(diff)  > m_maxFloating)
						ApplyGravityForce(point, m_maxFloating/1);
					else
						ApplyGravityForce(point, Mathf.Abs(diff/1) );
				}
			}
		}
		m_shipVelocity = m_shipRB.velocity;

		/*if(m_shipMagnitude > m_maxVelocity){
			m_shipRB.velocity = Vector3.ClampMagnitude(m_shipRB.velocity, m_maxVelocity);
		}*/
	}
}
