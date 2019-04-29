using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canon : MonoBehaviour {

    [SerializeField] public bool m_Left;
    [SerializeField] private Interact CanonZone;

    [Header("Shooting")]
    [SerializeField] public Transform m_shootPoint;
    [SerializeField] private float m_bulletSpeed;
    [SerializeField] private GameObject m_canonBall;

    [Header("Reloading")]
    [SerializeField] private bool StartLoaded;
    [SerializeField] private float reloadSpeed;
    [SerializeField] public bool isLoaded;

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


    [Header("UI")]
    [SerializeField] private GameObject m_Bar;

    private float CanonRotation_X = 0f;
    private float CanonRotation_Y = 0f;
	private Curve curve;
	private bool isReloading;
	private bool isLineActive;

	void Start()
	{
		curve = transform.GetComponent<Curve>();
		//curve.m_line.enabled = false;

		if(StartLoaded){
			isLoaded = true;
			m_Bar.transform.localScale = new Vector3(1,1,1);
		}else{
			isLoaded = false;
			m_Bar.transform.localScale = new Vector3(0,1,1);
		}
	}


	void Update()
	{
		if(CanonZone.isZoneActive()){
			
			if(!isLineActive)
				curve.m_line.enabled = true;
			float m_inputV = Input.GetAxisRaw("Vertical");
			float m_inputH = Input.GetAxisRaw("Horizontal");

			//move Down
			if (m_inputV > 0)
			{
				CanonRotation_X = VerticalPivotTransform.localEulerAngles.x + verticalRotationSpeed;

				if (CanonRotation_X > (rangeAngleX/2 - offsetX) && CanonRotation_X < 270f)
				{
					CanonRotation_X = (rangeAngleX/2 - offsetX);
				}

				VerticalPivotTransform.localEulerAngles = new Vector3(CanonRotation_X, 180, 0f);
			}

			//move Up
			else if (m_inputV < 0)
			{
				CanonRotation_X = VerticalPivotTransform.localEulerAngles.x - verticalRotationSpeed;

				if (CanonRotation_X < 360-(rangeAngleX/2 + offsetX) && CanonRotation_X > 90f)
				{
					CanonRotation_X = 360-(rangeAngleX/2 + offsetX);
				}

				VerticalPivotTransform.localEulerAngles = new Vector3(CanonRotation_X, 180, 0f);
			}

			//move Left
			// if (m_inputH > 0)
			// {
			// 	CanonRotation_Y = HorizontalPivotTransform.localEulerAngles.y + horizontalRotationSpeed;

			// 	if (CanonRotation_Y > rangeAngleY/2 && CanonRotation_Y < 270f)
			// 	{
			// 		CanonRotation_Y = rangeAngleY/2;
			// 	}

			// 	HorizontalPivotTransform.localEulerAngles = new Vector3(0, CanonRotation_Y, 0f);
			// }

			// //move Right
			// else if (m_inputH < 0)
			// {
			// 	CanonRotation_Y = HorizontalPivotTransform.localEulerAngles.y - horizontalRotationSpeed;

			// 	if (CanonRotation_Y < 360-rangeAngleY/2 && CanonRotation_Y > 90f)
			// 	{
			// 		CanonRotation_Y = 360-rangeAngleY/2;
			// 	}

			// 	HorizontalPivotTransform.localEulerAngles = new Vector3(0, CanonRotation_Y, 0f);
			// }
		}
		else{
			if(isLineActive)
				curve.m_line.enabled = false;
		}
	}

	public void ShowLine(){

	}

	public void HideLine(){

	}

	public void CanonReload(bool left){
		if(!isReloading && !isLoaded && (left == m_Left))
			StartCoroutine("ReloadCor");
	}

	private IEnumerator ReloadCor(){
		isReloading = true;

		m_Bar.transform.localScale = new Vector3(0,1,1);
		while(m_Bar.transform.localScale.x < 1){
			m_Bar.transform.localScale += new Vector3(0.01f,0,0);
			yield return new WaitForSeconds(1/reloadSpeed);
		}

		m_Bar.transform.localScale = new Vector3(1,1,1);

		isLoaded = true;
		isReloading = false;
	}

	public void CanonShoot(){
		if(isLoaded){
			print(gameObject.name + " shoot");
			GameObject bullet = Instantiate(m_canonBall, m_shootPoint.position, m_shootPoint.rotation, transform);
			bullet.name = "CanonBall";

			bullet.GetComponent<CurveWalker>().m_curve = curve;
			bullet.GetComponent<CurveWalker>().m_travelTime = m_bulletSpeed;
			bullet.GetComponent<CurveWalker>().launch = true;
			/*
			bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * m_bulletSpeed, ForceMode.Impulse);
			if(m_Left){
				//bullet.GetComponent<ConstantForce>().force = new Vector3(transform.parent.parent.parent.GetComponent<Rigidbody>().velocity.magnitude, 0,0);
				bullet.GetComponent<ConstantForce>().force = new Vector3(transform.GetComponentInParent<Rigidbody>().velocity.magnitude, 0,0);
			}else{
				bullet.GetComponent<ConstantForce>().force = new Vector3(-transform.GetComponentInParent<Rigidbody>().velocity.magnitude, 0,0);
			}
			*/

			isLoaded = false;
			m_Bar.transform.localScale = new Vector3(0,1,1);
		}
	}
}
