using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canon : MonoBehaviour {

    //[Header("Aiming")]
    //[SerializeField] public float m_maxHeight;
    [SerializeField] public bool m_Left;
	
    //[Header("Vertical Rotation")]
    //[SerializeField] private float verticalRotationSpeed = 0.8f;
    //[SerializeField] private float rangeAngleX = 60;
    //[SerializeField] private float offsetX = 0;

    //[Header("Horizontal Rotation")]
    //[SerializeField] private float horizontalRotationSpeed = 0.8f;
    //[SerializeField] private float rangeAngleY = 60;    
	
	//[Header("Shooting")]
    //[SerializeField] private float m_bulletSpeed;
    //[SerializeField] private GameObject m_canonBall;

    //[Header("Reloading")]
    //[SerializeField] private bool StartLoaded = true;
    //[SerializeField] private float reloadSpeed;
    [SerializeField] public bool isLoaded;

    [Header("Refs")]
    [SerializeField] public Transform VerticalPivotTransform;
    [SerializeField] public Transform HorizontalPivotTransform;
    [SerializeField] public Transform[] m_shootPoint;
    [SerializeField] private Interact CanonZone;
    [SerializeField] private GameObject m_UIBar;

	private bool isReloading;
	private CanonManager m_Manager;
	private AudioSource m_source;
    private float CanonRotation_X = 0f;
    private float CanonRotation_Y = 0f;
	//private Curve curve;
	//private bool isLineActive;

	void Start()
	{
		//curve = transform.GetComponent<Curve>();		
		m_Manager = transform.GetComponentInParent<CanonManager>();
		m_source = GetComponent<AudioSource>();

		if(m_Manager.StartLoaded){
			isLoaded = true;
			if(m_UIBar != null)
				m_UIBar.transform.localScale = new Vector3(1,1,1);
		}else{
			isLoaded = false;
			if(m_UIBar != null)
				m_UIBar.transform.localScale = new Vector3(0,1,1);
		}
	}


	void Update()
	{
		if(CanonZone.isZoneActive()){
			InputRotation();
		}
		else{
			//if(isLineActive)
				//curve.m_line.enabled = false;
		}

	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//ROTATION

	private void InputRotation(){
		//if(!isLineActive)
			//curve.m_line.enabled = true;
		float m_inputV = Input.GetAxisRaw("Vertical");
		float m_inputH = Input.GetAxisRaw("Horizontal");

		//move Down
		if (m_inputV > 0)
		{	
			RotateDown();
			//transform.parent.BroadcastMessage("SetRotationUp", VerticalPivotTransform.rotation, SendMessageOptions.DontRequireReceiver);
			foreach (Transform child in transform.parent)
			{
				if(child.GetComponent<Canon>() && child.name != gameObject.name){
					child.GetComponent<Canon>().SetRotationUp(VerticalPivotTransform.localRotation);
				}
			}
		}

		//move Up
		else if (m_inputV < 0)
		{
			RotateUp();
			//transform.parent.BroadcastMessage("SetRotationUp", VerticalPivotTransform.rotation, SendMessageOptions.DontRequireReceiver);				
			foreach (Transform child in transform.parent)
			{
				if(child.GetComponent<Canon>() && child.name != gameObject.name){
					child.GetComponent<Canon>().SetRotationUp(VerticalPivotTransform.localRotation);
				}
			}
		}

		//move Left
		if (m_inputH > 0)
		{
			RotateLeft();
			//transform.parent.BroadcastMessage("SetRotationRight", HorizontalPivotTransform.rotation, SendMessageOptions.DontRequireReceiver);
			foreach (Transform child in transform.parent)
			{
				if(child.GetComponent<Canon>() && child.name != gameObject.name){
					child.GetComponent<Canon>().SetRotationRight(HorizontalPivotTransform.localRotation);
				}
			}
		}

		//move Right
		else if (m_inputH < 0)
		{
			RotateRight();
			//transform.parent.BroadcastMessage("SetRotationRight", HorizontalPivotTransform.rotation, SendMessageOptions.DontRequireReceiver);
			foreach (Transform child in transform.parent)
			{
				if(child.GetComponent<Canon>() && child.name != gameObject.name){
					child.GetComponent<Canon>().SetRotationRight(HorizontalPivotTransform.localRotation);
				}
			}
		}
	}


	public void RotateRight(){
		CanonRotation_Y = HorizontalPivotTransform.localEulerAngles.y - m_Manager.horizontalRotationSpeed * Time.deltaTime;

		if (CanonRotation_Y < 360-m_Manager.rangeAngleY/2 && CanonRotation_Y > 90f)
		{
			CanonRotation_Y = 360-m_Manager.rangeAngleY/2;
		}

		HorizontalPivotTransform.localEulerAngles = new Vector3(0, CanonRotation_Y, 0f);

	}

	public void RotateLeft(){
		CanonRotation_Y = HorizontalPivotTransform.localEulerAngles.y + m_Manager.horizontalRotationSpeed * Time.deltaTime;

		if (CanonRotation_Y > m_Manager.rangeAngleY/2 && CanonRotation_Y < 270f)
		{
			CanonRotation_Y = m_Manager.rangeAngleY/2;
		}

		HorizontalPivotTransform.localEulerAngles = new Vector3(0, CanonRotation_Y, 0f);
	}

	public void RotateUp(){
		CanonRotation_X = VerticalPivotTransform.localEulerAngles.x - m_Manager.verticalRotationSpeed * Time.deltaTime;

		if (CanonRotation_X < 360-(m_Manager.rangeAngleX/2 + m_Manager.offsetX) && CanonRotation_X > 90f)
		{
			CanonRotation_X = 360-(m_Manager.rangeAngleX/2 + m_Manager.offsetX);
		}

		VerticalPivotTransform.localEulerAngles = new Vector3(CanonRotation_X, 180, 0f);
	}

	public void RotateDown(){
		CanonRotation_X = VerticalPivotTransform.localEulerAngles.x + m_Manager.verticalRotationSpeed * Time.deltaTime;

		if (CanonRotation_X > (m_Manager.rangeAngleX/2 - m_Manager.offsetX) && CanonRotation_X < 270f)
		{
			CanonRotation_X = (m_Manager.rangeAngleX/2 - m_Manager.offsetX);
		}

		VerticalPivotTransform.localEulerAngles = new Vector3(CanonRotation_X, 180, 0f);
	}

	public void SetRotationUp(Quaternion m_localRotationUp){
		VerticalPivotTransform.localRotation = m_localRotationUp;
	}

	public void SetRotationRight(Quaternion m_localRotationRight){
		HorizontalPivotTransform.localRotation = m_localRotationRight;
	}

	public void SetAngleUp(float angle){
		VerticalPivotTransform.localRotation = Quaternion.Euler(-angle,180,0);
	}
	public void SetAngleRight(float angle){
		HorizontalPivotTransform.localRotation = Quaternion.Euler(0,angle,0);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//SHOOT

	public void CanonReload(int nbCannons){
		if(!isReloading && !isLoaded)
			StartCoroutine("ReloadCor", nbCannons);
	}


	private IEnumerator ReloadCor(int nbCannons){
		isReloading = true;

		m_UIBar.transform.localScale = new Vector3(0,1,1);
		while(m_UIBar.transform.localScale.x < 1){
			m_UIBar.transform.localScale += new Vector3(0.01f,0,0);
			yield return new WaitForSeconds(1/(m_Manager.reloadSpeed/nbCannons));
		}

		m_UIBar.transform.localScale = new Vector3(1,1,1);

		isLoaded = true;
		isReloading = false;
	}

	public void CanonShoot(){
		if(isLoaded){
			//print(gameObject.name + " shoot");
			foreach (Transform shootpoint in m_shootPoint)
			{			
				StartCoroutine("DelayShoot", shootpoint);
			}

			isLoaded = false;
			m_UIBar.transform.localScale = new Vector3(0,1,1);
		}
	}

	private IEnumerator DelayShoot(Transform point){
		//Delay
		yield return new WaitForSeconds(Random.Range(0, 0.5f));

		//Spawn bullet
		GameObject bullet = Instantiate(m_Manager.m_canonBall, point.position, point.rotation, null);
		bullet.name = "CanonBall";

		//Sound
		if(m_source.priority != m_Manager.m_priority)
			m_source.priority = m_Manager.m_priority;
		if(m_source != null)
			m_source.PlayOneShot(m_Manager.CanonsClips[Random.Range(0, m_Manager.CanonsClips.Length)], m_Manager.m_volume);

		//Play FX
		if(point.GetChild(0).GetComponent<ParticleSystem>() != null)
			point.GetChild(0).GetComponent<ParticleSystem>().Play(true);

		//Add main force in Bullet
		bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * m_Manager.m_bulletSpeed, ForceMode.Impulse);


		//Add secondary force in Bullet and Recoil		
		
		//transform.GetComponentInParent<Rigidbody>().AddForceAtPosition(-bullet.transform.forward * m_ship.m_bulletSpeed * m_ship.m_ForceCanonMultiplier, transform.position, ForceMode.Acceleration);
		if(m_Left){			
			transform.GetComponentInParent<Rigidbody>().AddTorque(-GetComponentInParent<FloatingShip>().transform.forward * m_Manager.m_bulletSpeed * m_Manager.m_ForceCanonMultiplier, ForceMode.Acceleration);

			//bullet.GetComponent<ConstantForce>().force = new Vector3(transform.GetComponentInParent<Rigidbody>().velocity.magnitude, 0,0);
		}else{
			transform.GetComponentInParent<Rigidbody>().AddTorque(GetComponentInParent<FloatingShip>().transform.forward * m_Manager.m_bulletSpeed * m_Manager.m_ForceCanonMultiplier, ForceMode.Acceleration);

			//bullet.GetComponent<ConstantForce>().force = new Vector3(-transform.GetComponentInParent<Rigidbody>().velocity.magnitude, 0,0);
		}
	}
}
