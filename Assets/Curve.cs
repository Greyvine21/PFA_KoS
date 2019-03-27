using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour {

	public Transform Shootpoint;
	public float Shotpower = 2;
	public float ShotRangeFactor = 2;
	public Vector3[] points;

	public LineRenderer m_line;
	public int LineDivision = 10;

	void Start()
	{
		if(!m_line)
			m_line = GetComponent<LineRenderer>();
		m_line.positionCount = LineDivision;
	}

	void Update()
	{
		points[0] = Shootpoint.position;
		points[1] = Shootpoint.position + Shootpoint.forward * Shotpower;
		points[2] = points[1];
		points[3] = transform.position + transform.forward * Shotpower*ShotRangeFactor;

		for (int i = 0; i < m_line.positionCount; i++)
		{
			print(GetPoint((1/LineDivision)*i));
			m_line.SetPosition(i, GetPoint((1/LineDivision)*i));
		}
	}

	// void OnValidate()
	// {
	// 	points[0] = Shootpoint.position;
	// 	points[1] = Shootpoint.position + Shootpoint.forward * Shotpower;
	// 	points[2] = points[1];
	// 	points[3] = transform.position + transform.forward * Shotpower*ShotRangeFactor;
	// }
	public void Reset(){
		points = new Vector3[]{
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
	}

	public Vector3 GetPoint(float t){
		return transform.TransformPoint(Bezier.GetPoint(points[0],points[1],points[2], points[3], t));
	}
	public Vector3 GetVelocity(float t){
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[0],points[1],points[2], points[3], t)) - transform.position;
	}

	public Vector3 GetDirection(float t){
		return GetVelocity(t).normalized;
	}
}
