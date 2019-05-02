using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Buoy{
	public Transform transform;
	public Vector3 direction;
	public bool applyGravityForce;
}

public enum SailsState{
	sailsZeroPerCent = 0,
	sailsFiftyPerCent,
	sailsHundredPerCent

}

public class FloatingShip : MonoBehaviour {

	[Header("References")]	
	[SerializeField] private Transform m_floatingHeight;
	[SerializeField] private Transform m_CenterOfMass;
	[SerializeField] private Controller3D m_Player;
	[SerializeField] private Ocean m_ocean;

	[Header("Floating force")]	
	[SerializeField] private float m_floatingCoef;
	[SerializeField] private float m_maxFloating = 4f;
	[SerializeField] private ForceMode m_force;
	//[SerializeField] private float m_maxVelocity = 4f;
	[SerializeField] private Buoy[] m_buoy;

	[Header("Turning force")]
    [SerializeField] private Transform FrontTransform;
    [SerializeField] private Transform rudderBladeTransform;
    [SerializeField] private Transform rudderTransform;
    [SerializeField] private Interact rudderInteractZone;
     private float rotationAngle = 30;
    [SerializeField] private float m_turningForce;
    [SerializeField] private float rotationRudderBlade = 2f;

	[Header("Sails")]
	[SerializeField] public SailsState m_sailsSate = SailsState.sailsZeroPerCent;
	[SerializeField] private float m_MaxForwardSpeed;
	[SerializeField] private float m_forwardAcceleration;
	[SerializeField] private Transform[] m_sails;
	[SerializeField] private float m_sailsUpSpeed;
	[SerializeField] private float m_sailsDownSpeed;

	[Header("Canons")]
	[SerializeField] public Transform m_canonsRight;
	[SerializeField] public Transform m_canonsleft;
    [SerializeField] public float m_bulletSpeed;
    [SerializeField] public float m_ForceCanonMultiplier;
    [SerializeField] public GameObject m_canonBall;
    [SerializeField] public float reloadSpeed;
    [SerializeField] public AudioClip[] CanonsClips;

	[Header("Brake")]
	[SerializeField] private float m_brakeImpulse = 30;
	[SerializeField] private float m_brakeForce = 1;

	[Header("Monitoring")]
	public bool anchorDown = false;
	[SerializeField] private float m_currentforwardSpeed;
	[SerializeField] private float m_waterLevel;
	[SerializeField] private Vector3 m_shipVelocity;

	//private float forward0, forward50, forward100;
	private float forward;
	private int ActiveCannonsRight, ActiveCannonsLeft;
	private bool sailsGoingUp, SailsGoingDown;
	//private bool isRudderActive = false;
	private bool shipSlowingDown = false;
	private SailsState m_sailsStateOLD;
    private float RudderBladeRotation_Y = 0f;
    private float RudderRotation_Z = 0f;
	private Rigidbody m_shipRB;


	void Start()
	{
		m_shipRB = GetComponent<Rigidbody>();
		//m_shipRB.centerOfMass = m_CenterOfMass.position;

		/*forward100 = m_MaxForwardSpeed;
		forward50 = m_MaxForwardSpeed/2;
		forward0 = m_MaxForwardSpeed/10;*/

		ActiveCannonsRight = 0;
		foreach (Transform child in m_canonsRight)
		{
			if(child.gameObject.activeSelf){
				ActiveCannonsRight ++;
			}
		}

		ActiveCannonsLeft = 0;
		foreach (Transform child in m_canonsleft)
		{
			if(child.gameObject.activeSelf){
				ActiveCannonsLeft ++;
			}
		}

		foreach (Transform sail in m_sails)
		{
			switch (m_sailsSate)
			{
			case SailsState.sailsFiftyPerCent:
				sail.localScale = new Vector3(1, 0.5f ,1);
			break;
			case SailsState.sailsHundredPerCent:
				sail.localScale = new Vector3(1, 1 ,1);
			break;			
			case SailsState.sailsZeroPerCent:
				sail.localScale = new Vector3(1, 0.04f ,1);
			break;
			default:
				sail.localScale = new Vector3(1, 10 ,1);
			break;
			}
		}
	}

	void Update() 
	{
        UserInput();
		SailsStateUpdate();
    }

	void FixedUpdate()
	{	
		//move
		// if(!shipSlowingDown){
		// 	AddMainForce(forward);
		// 	TurningForce();
		// }

		//float
		Float();

		//Anchor
		if(anchorDown){
			m_shipRB.velocity = Vector3.zero;
		}else{
			AddMainForce(forward);
			TurningForce();
		}
	}

	//INPUT 
    void UserInput()
    {
        //Forward / reverse
		if (Input.GetKeyDown(KeyCode.UpArrow))
        {
			OrderSailsUp();
        }		
		if (Input.GetKeyDown(KeyCode.DownArrow))
        {
			OrderSailsDown();
        }

		float m_inputH = Input.GetAxisRaw("Horizontal");

        //Steer left
        if (Input.GetKey(KeyCode.LeftArrow) || (m_inputH < 0 && rudderInteractZone.isZoneActive()))
        {
            RudderBladeRotation_Y = rudderBladeTransform.localEulerAngles.y + rotationRudderBlade;

            if (RudderBladeRotation_Y > rotationAngle && RudderBladeRotation_Y < 270f)
            {
                RudderBladeRotation_Y = rotationAngle;
                RudderRotation_Z = 0;
            }
			else{
				RudderRotation_Z = rudderTransform.localEulerAngles.z + (rotationRudderBlade * (360/rotationAngle));
			}

            Vector3 newRotationRudderBlade = new Vector3(0f, RudderBladeRotation_Y, 0f);
            Vector3 newRotationRudder = new Vector3(0f, 0f, RudderRotation_Z);

            rudderBladeTransform.localEulerAngles = newRotationRudderBlade;
			FrontTransform.localEulerAngles = - newRotationRudderBlade;
			rudderTransform.localEulerAngles = newRotationRudder;
        }

        //Steer right
        else if (Input.GetKey(KeyCode.RightArrow) || (m_inputH > 0 && rudderInteractZone.isZoneActive()))
        {
            RudderBladeRotation_Y = rudderBladeTransform.localEulerAngles.y - rotationRudderBlade;

            if (RudderBladeRotation_Y < 360-rotationAngle && RudderBladeRotation_Y > 90f)
            {
                RudderBladeRotation_Y = 360-rotationAngle;
				RudderRotation_Z = 0;
            }
			else{
				RudderRotation_Z = rudderTransform.localEulerAngles.z - (rotationRudderBlade * (360/rotationAngle));
			}

            Vector3 newRotationRudderBlade = new Vector3(0f, RudderBladeRotation_Y, 0f);
            Vector3 newRotationRudder = new Vector3(0f, 0f, RudderRotation_Z);

            rudderBladeTransform.localEulerAngles = newRotationRudderBlade;
			FrontTransform.localEulerAngles = -newRotationRudderBlade;
			rudderTransform.localEulerAngles = newRotationRudder;
        }
    }

	//SAILS

	public bool OrderSailsUp(){
		if (m_sailsSate != SailsState.sailsZeroPerCent && !sailsGoingUp)
        {
			m_sailsSate--;
			return true;
        }
		return false;
	}

	public bool OrderSailsDown(){
		if (m_sailsSate != SailsState.sailsHundredPerCent && !SailsGoingDown)
        {
			m_sailsSate++;
			return true;
        }
		return false;
	}

	private void SailsStateUpdate()
	{
		if(m_sailsSate != m_sailsStateOLD)
		{

			switch (m_sailsSate)
			{
			case SailsState.sailsZeroPerCent:
				forward = m_MaxForwardSpeed/5;
				StopCoroutine("SailsDown");
				SailsGoingDown = false;
				StartCoroutine("SailsUp");
			break;
			case SailsState.sailsFiftyPerCent:
				forward = m_MaxForwardSpeed/2;
				if(m_sailsStateOLD > m_sailsSate)
				{
					StopCoroutine("SailsDown");
					SailsGoingDown = false;
					StartCoroutine("SailsUp");
				}
				else
				{
					StopCoroutine("SailsUp");
					sailsGoingUp = false;
					StartCoroutine("SailsDown");
				}

			break;
			case SailsState.sailsHundredPerCent:
				forward = m_MaxForwardSpeed;
				StopCoroutine("SailsUp");
				sailsGoingUp = false;
				StartCoroutine("SailsDown");
			break;
			default:
				AddMainForce(0);
			break;
			}
			
			m_sailsStateOLD = m_sailsSate;
		}
	}
	IEnumerator SailsUp(){
		sailsGoingUp = true;
		//m_sailsSate--;
		float MinScale;
		switch (m_sailsSate)
		{
			case SailsState.sailsFiftyPerCent:
				MinScale = 0.5f;
			break;
			case SailsState.sailsZeroPerCent:
				MinScale = 0.04f;
			break;
			default:
				MinScale = 1.5f;
			break;
		}

		while (m_sails[0].localScale.y > MinScale)
		{
			foreach (Transform sail in m_sails)
			{
				sail.localScale -= new Vector3(0, 0.01f ,0);
				yield return new WaitForSeconds(1/m_sailsUpSpeed);
			}
		}
		
		sailsGoingUp = false;
	}

	IEnumerator SailsDown(){
		SailsGoingDown = true;
		//m_sailsSate++;
		float MaxScale;
		switch (m_sailsSate)
		{
			case SailsState.sailsFiftyPerCent:
				MaxScale = 0.5f;
			break;
			case SailsState.sailsHundredPerCent:
				MaxScale = 1f;
			break;
			default:
				MaxScale = 1.5f;
			break;
		}

		while (m_sails[0].localScale.y < MaxScale)
		{
			foreach (Transform sail in m_sails)
			{
				sail.localScale += new Vector3(0, 0.05f ,0);
				yield return new WaitForSeconds(1/m_sailsDownSpeed);
			}
		}
		
		SailsGoingDown = false;
	}

	//CANONS

	public bool ReloadCanonLeft(){

		foreach (Transform child in m_canonsleft)
		{
			if(child.GetComponent<Shoot_Canon>() && child.gameObject.activeSelf){
				if(child.GetComponent<Shoot_Canon>().isLoaded){
					return false;
				}
				child.GetComponent<Shoot_Canon>().CanonReload(ActiveCannonsLeft);
			}
		}
		return true;
	}

	public bool ReloadCanonRight(){

		foreach (Transform child in m_canonsRight)
		{
			if(child.GetComponent<Shoot_Canon>() && child.gameObject.activeSelf){
				if(child.GetComponent<Shoot_Canon>().isLoaded){
					return false;
				}
				child.GetComponent<Shoot_Canon>().CanonReload(ActiveCannonsRight);
			}
		}
		return true;
	}
	//MOVEMENT
	void AddMainForce(float maxSpeed)
	{
		if(m_currentforwardSpeed < maxSpeed && !shipSlowingDown)
		{
			m_currentforwardSpeed += m_forwardAcceleration;
		}		
		else if(m_currentforwardSpeed > maxSpeed || shipSlowingDown)
		{
			m_currentforwardSpeed -= m_forwardAcceleration*10;
		}

		if(m_currentforwardSpeed < 0)
			m_currentforwardSpeed = 0;
		Vector3 mainForce = m_CenterOfMass.forward * m_currentforwardSpeed;

		m_shipRB.AddForceAtPosition(mainForce, m_CenterOfMass.position, m_force);
		//
        Debug.DrawRay(m_CenterOfMass.position, mainForce, Color.red);
	}

    void TurningForce()
    {
        Vector3 turningForceTemp;
		switch (m_sailsSate)
		{
			case SailsState.sailsZeroPerCent:
				turningForceTemp = rudderBladeTransform.forward * m_turningForce * 1;
			break;
			case SailsState.sailsFiftyPerCent:
				turningForceTemp = rudderBladeTransform.forward * m_turningForce * 0.6f;
			break;
			case SailsState.sailsHundredPerCent:
				turningForceTemp = rudderBladeTransform.forward * m_turningForce * 0.4f;
			break;
			default:
				turningForceTemp = Vector3.zero;
			break;
		}

		if((RudderBladeRotation_Y > 5 && RudderBladeRotation_Y < 355) && !anchorDown)
		{
			m_shipRB.AddForceAtPosition(-turningForceTemp*0.75f, FrontTransform.position, m_force);
        	Debug.DrawRay(FrontTransform.position, turningForceTemp, Color.red);
			//
			m_shipRB.AddForceAtPosition(turningForceTemp, rudderBladeTransform.position, m_force);
        	Debug.DrawRay(rudderBladeTransform.position, turningForceTemp, Color.red);
		}
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
			m_waterLevel = -3;
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

	//OTHER
	void AddImpulseAtFloatingPoint(int index, Vector3 direction, float magnitude, ForceMode force){
		m_shipRB.AddForceAtPosition(direction*magnitude, m_buoy[index].transform.position, ForceMode.VelocityChange);
	}
	
	//ANCHOR/BRAKE
	public bool Anchor(){
		if(shipSlowingDown){
			return false;
		}else{
			if(anchorDown){
				m_shipRB.constraints = RigidbodyConstraints.None;
				anchorDown = false;
			}else{
				StartCoroutine("SlowDownShip");
			}
			return true;
		}
	}

	IEnumerator SlowDownShip(){
		shipSlowingDown = true;
		while(m_shipRB.velocity.z > 1){
			m_shipRB.velocity -= new Vector3(0,0,m_brakeForce);
			AddImpulseAtFloatingPoint(0, Vector3.down, m_brakeImpulse*m_shipRB.velocity.z, m_force);
			yield return new WaitForSeconds(0.1f);
		}
		//AddImpulseAtFloatingPoint(0, Vector3.down, m_brakeImpulse, ForceMode.VelocityChange);
		//m_shipRB.AddTorque(1,0,0, ForceMode.VelocityChange);
		anchorDown = true;
		shipSlowingDown = false;
		m_shipRB.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		print("ship stopped");
	}

	//DEBUG
	void OnDrawGizmos()
	{
		//Buoy
		Gizmos.color = Color.green;
		foreach (Buoy point in m_buoy)
		{
			Gizmos.DrawWireSphere(point.transform.position, 1);
			//Vector3 tmp = new Vector3(point.transform.position.x, m_waterLevel ,point.transform.position.z);
			//Gizmos.DrawLine(point.transform.position, tmp);
		}
		

		//Height
		//Gizmos.color = Color.white;
		//int L = 2;
		//Gizmos.DrawLine(m_floatingHeight.position + Vector3.forward*L, m_floatingHeight.position - Vector3.forward*L);
		//Gizmos.DrawLine(m_floatingHeight.position + Vector3.right*L, m_floatingHeight.position - Vector3.right*L);

		//Velocity
		//Gizmos.color = Color.red;
		//Gizmos.DrawRay(transform.position, m_shipRB.velocity);
		
	}
}
