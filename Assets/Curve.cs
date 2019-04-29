using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour {
    public Canon m_canon;

    [Header("Curve parameters")]
	public float forward;
	public float up;

    [Header("Line")]
	public LineRenderer m_line;
	public int LinePrecision = 10;

    [Header("Monitoring")]
	public float angleCanon;
	public Vector3[] points;
	private Transform Shootpoint;


	void Start()
	{
		if(!m_canon)
			m_canon = GetComponent<Canon>();
		if(!m_line)
			m_line = GetComponent<LineRenderer>();
		
		Shootpoint = m_canon.m_shootPoint;
		m_line.positionCount = LinePrecision;
	}

	void Update()
	{
		Transform origin = m_canon.HorizontalPivotTransform;
		float CanonRotationXEuler = m_canon.VerticalPivotTransform.localEulerAngles.x;

		//
		if(CanonRotationXEuler > 0 && CanonRotationXEuler < 180){
			angleCanon = - CanonRotationXEuler;
		}
		else{
			angleCanon = 360 - CanonRotationXEuler;
		}

		//float P2_X = 2 * m_canon.m_maxHeight * (Mathf.Sin( angleCanon*Mathf.Deg2Rad)/9.81f);
		//float P1_X = Mathf.Tan(angleCanon*Mathf.Deg2Rad) *(P2_X/2);
		//float P1_Y = P1_X* ;

		//print("Theta : " + angleCanon + "    P2.X : " + P2_X + "    P1.x : " + P1_X);

		//
		points[0] = Shootpoint.position;
		/*
		points[1] = origin.position + Shootpoint.forward * (P1_X/Mathf.Cos(angleCanon*Mathf.Deg2Rad));
		points[2] = origin.position + new Vector3(P2_X,0,0);
		/*/
		points[1] = Shootpoint.position + Shootpoint.forward * m_canon.m_maxHeight;
		//points[2] = new Vector3((points[1].z - points[0].z)*2, origin.position.y, origin.position.x);
		print("Angle : " + angleCanon + "   Cos : " + Mathf.Cos(angleCanon) + "   length : " + Mathf.Abs(m_canon.m_maxHeight* Mathf.Cos(angleCanon)));
		points[2] = origin.position + origin.forward * Mathf.Abs(m_canon.m_maxHeight* Mathf.Cos(angleCanon));
		//points[2] = origin.position + origin.forward + new Vector3(Mathf.Cos(angleCanon*Mathf.Deg2Rad), Mathf.Sin(angleCanon*Mathf.Deg2Rad), 0) * m_canon.m_maxHeight; 

		//points[2] = new Vector3(points[1].x, 0, points[1].z) +
					//m_canon.HorizontalPivotTransform.forward * (points[1].z*2);

		//*/

		points[3] = points[2];



		for (int i = 0; i < m_line.positionCount; i++)
		{
			//print("for " + (1/LineDivision)*i + " : " + GetPoint((1/LineDivision)*i));
			//m_line.SetPosition(i, transform.InverseTransformPoint(GetWorldPoint((1/(float)LinePrecision)*i)));
			m_line.SetPosition(i, GetLocalPoint((1/(float)LinePrecision)*i));
		}
	}

	// void OnValidate()
	// {
	// 	//
	// 	points[0] = Shootpoint.position;
	// 	/*
	// 	points[1] = origin.position + Shootpoint.forward * (P1_X/Mathf.Cos(angleCanon*Mathf.Deg2Rad));
	// 	points[2] = origin.position + new Vector3(P2_X,0,0);
	// 	/*/
	// 	points[1] = Shootpoint.position + Shootpoint.forward * m_canon.m_maxHeight;
	// 	points[2] = new Vector3(points[1].x + Mathf.Sin(angleCanon*Mathf.Deg2Rad)*m_canon.m_maxHeight, points[1].y - Mathf.Cos(angleCanon*Mathf.Deg2Rad)*m_canon.m_maxHeight, points[1].z );
	// 	//points[2] = origin.position + origin.forward + new Vector3(Mathf.Cos(angleCanon*Mathf.Deg2Rad), Mathf.Sin(angleCanon*Mathf.Deg2Rad), 0) * m_canon.m_maxHeight; 

	// 	//points[2] = new Vector3(points[1].x, 0, points[1].z) +
	// 				//m_canon.HorizontalPivotTransform.forward * (points[1].z*2);

	// 	//*/

	// 	points[3] = points[2];
	// }

	public void Reset(){
		points = new Vector3[]{
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
	}

	public Vector3 GetWorldPoint(float t){
		return transform.TransformPoint(GetLocalPoint(t));
	}

	public Vector3 GetLocalPoint(float t){
		return Bezier.GetPoint(points[0],points[1],points[2], points[3], t);
	}

	public Vector3 GetVelocity(float t){
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[0],points[1],points[2], points[3], t)) - transform.position;
	}

	public Vector3 GetDirection(float t){
		return GetVelocity(t).normalized;
	}
}
