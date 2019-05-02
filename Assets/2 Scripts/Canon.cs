using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canon : MonoBehaviour {
    [SerializeField] private Interact CanonZone;

    [Header("Aiming")]
    [SerializeField] public float m_maxHeight;
	
    [Header("Vertical Rotation")]
    [SerializeField] public Transform VerticalPivotTransform;
    [SerializeField] private float verticalRotationSpeed = 0.8f;
    [SerializeField] private float rangeAngleX = 60;
    [SerializeField] private float offsetX = 0;

    [Header("Horizontal Rotation")]
    [SerializeField] public Transform HorizontalPivotTransform;
    [SerializeField] private float horizontalRotationSpeed = 0.8f;
    [SerializeField] private float rangeAngleY = 60;

    private float CanonRotation_X = 0f;
    private float CanonRotation_Y = 0f;
	private Curve curve;
	private bool isLineActive;
	private bool m_isLeft;

	void Start()
	{
		curve = transform.GetComponent<Curve>();
		//m_isLeft = GetComponent<Shoot_Canon>().m_Left;
		//curve.m_line.enabled = false;
	}


	void Update()
	{
		if(CanonZone.isZoneActive()){
			
			//if(!isLineActive)
				//curve.m_line.enabled = true;
			float m_inputV = Input.GetAxisRaw("Vertical");
			float m_inputH = Input.GetAxisRaw("Horizontal");

			//move Down
			if (m_inputV > 0)
			{	
				RotateDown();
				transform.parent.BroadcastMessage("SetRotationUp", VerticalPivotTransform.rotation, SendMessageOptions.DontRequireReceiver);
				//transform.parent.BroadcastMessage("RotateDown", m_isLeft, SendMessageOptions.DontRequireReceiver);
			}

			//move Up
			else if (m_inputV < 0)
			{
				RotateUp();
				transform.parent.BroadcastMessage("SetRotationUp", VerticalPivotTransform.rotation, SendMessageOptions.DontRequireReceiver);
				//transform.parent.BroadcastMessage("RotateUp", m_isLeft, SendMessageOptions.DontRequireReceiver);
			}

			//move Left
			if (m_inputH > 0)
			{
				RotateLeft();
				transform.parent.BroadcastMessage("SetRotationRight", HorizontalPivotTransform.rotation, SendMessageOptions.DontRequireReceiver);
				//transform.parent.BroadcastMessage("RotateLeft", m_isLeft, SendMessageOptions.DontRequireReceiver);
			}

			//move Right
			else if (m_inputH < 0)
			{
				RotateRight();
				transform.parent.BroadcastMessage("SetRotationRight", HorizontalPivotTransform.rotation, SendMessageOptions.DontRequireReceiver);
				//transform.parent.BroadcastMessage("RotateRight", m_isLeft, SendMessageOptions.DontRequireReceiver);
			}
		}
		else{
			if(isLineActive)
				curve.m_line.enabled = false;
		}
	}
	
	public void RotateRight(){
		//if(m_isLeft == isLeft){
			CanonRotation_Y = HorizontalPivotTransform.localEulerAngles.y - horizontalRotationSpeed;

			if (CanonRotation_Y < 360-rangeAngleY/2 && CanonRotation_Y > 90f)
			{
				CanonRotation_Y = 360-rangeAngleY/2;
			}

			HorizontalPivotTransform.localEulerAngles = new Vector3(0, CanonRotation_Y, 0f);
		//}
	}

	public void RotateLeft(){
		//if(m_isLeft == isLeft){
			CanonRotation_Y = HorizontalPivotTransform.localEulerAngles.y + horizontalRotationSpeed;

			if (CanonRotation_Y > rangeAngleY/2 && CanonRotation_Y < 270f)
			{
				CanonRotation_Y = rangeAngleY/2;
			}

			HorizontalPivotTransform.localEulerAngles = new Vector3(0, CanonRotation_Y, 0f);
		//}
	}

	public void RotateUp(){
		//if(m_isLeft == isLeft){
			CanonRotation_X = VerticalPivotTransform.localEulerAngles.x - verticalRotationSpeed;

			if (CanonRotation_X < 360-(rangeAngleX/2 + offsetX) && CanonRotation_X > 90f)
			{
				CanonRotation_X = 360-(rangeAngleX/2 + offsetX);
			}

			VerticalPivotTransform.localEulerAngles = new Vector3(CanonRotation_X, 180, 0f);
		//}
	}

	public void RotateDown(){
		//if(m_isLeft == isLeft){	
			CanonRotation_X = VerticalPivotTransform.localEulerAngles.x + verticalRotationSpeed;

			if (CanonRotation_X > (rangeAngleX/2 - offsetX) && CanonRotation_X < 270f)
			{
				CanonRotation_X = (rangeAngleX/2 - offsetX);
			}

			VerticalPivotTransform.localEulerAngles = new Vector3(CanonRotation_X, 180, 0f);
		//}
	}

	public void SetRotationUp(Quaternion rotUp){
		VerticalPivotTransform.rotation = rotUp;
	}

	public void SetRotationRight(Quaternion rotRight){
		HorizontalPivotTransform.rotation = rotRight;
	}
}
