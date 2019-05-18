using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Curve))]
public class Curve_Editor : Editor {

	private Curve curve;
	private Transform handleTransform;
	private Quaternion handleRotation;

	private const int lineSteps = 10;
	private const float directionScale = 0.5f;

	private void OnSceneGUI(){
		curve = target as Curve;
		handleTransform = curve.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

		Handles.color = Color.green;
		Vector3 p0 = curve.points[0];
		Handles.DrawWireDisc(p0, Vector3.up, 0.2f);

		Vector3 p1 = curve.points[1];
		Handles.DrawWireDisc(p1, Vector3.up, 0.2f);
		
		Vector3 p2 = curve.points[2];
		Handles.DrawWireDisc(p2, Vector3.up, 0.2f);

		Vector3 p3 = curve.points[3];
		Handles.DrawWireDisc(p3, Vector3.up, 0.2f);

		//Vector3 p2 = ShowPoint(2);
		//Vector3 p3 = ShowPoint(3);

		Handles.color = Color.gray;
		Handles.DrawLine(p0, p1);
		Handles.DrawLine(p1, p2);

		//ShowDirection();
		Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);

		// Handles.color = Color.white;
		// Vector3 lineStart = curve.GetPoint(0f);
		// Handles.color = Color.blue;
		// Handles.DrawLine(lineStart, lineStart+curve.GetDirection(0f));
		// for (int i = 0; i <= lineSteps; i++)
		// {
		// 	Handles.color = Color.white;
		// 	Vector3 lineEnd = curve.GetPoint(i/ (float)lineSteps);
		// 	Handles.DrawLine(lineStart, lineEnd);
		// 	Handles.color = Color.blue;
		// 	Handles.DrawLine(lineStart, lineStart+curve.GetDirection(0f));
		// 	lineStart = lineEnd;
		// }
	}

	private void ShowDirection(){
		Handles.color = Color.green;
		Vector3 point = curve.GetWorldPoint(0f);
		Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
		for (int i = 1; i <= lineSteps; i++) {
			point = curve.GetWorldPoint(i / (float)lineSteps);
			Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
		}
	}
	
	private Vector3 ShowPoint(int index){
		
		Vector3 point = handleTransform.TransformPoint(curve.points[index]);
		EditorGUI.BeginChangeCheck();
		point = Handles.DoPositionHandle(point, handleRotation);

		if(EditorGUI.EndChangeCheck()){
			Undo.RecordObject(curve, "Move Point");
			EditorUtility.SetDirty(curve);
			curve.points[index] = handleTransform.InverseTransformPoint(point);
		}
		return point;
	}
}
#endif