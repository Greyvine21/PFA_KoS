using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Buoy{
	public Transform transform;
	public Vector3 direction;
	public bool applyGravityForce;
}


public class FloatingShip : MonoBehaviour {

	#region Fields
	[SerializeField] protected bool gizmo;

	[Header("References")]
	[SerializeField] protected Transform m_CenterOfMass;
	[SerializeField] protected Ocean m_ocean;

	[Header("Floating force")]
	[SerializeField] protected bool useOcean;
	[SerializeField] protected float m_floatingCoef;
	[SerializeField] protected float m_maxFloating = 4f;
	[SerializeField] protected ForceMode m_force;
	//[SerializeField] private float m_maxVelocity = 4f;
	[SerializeField] protected Buoy[] m_buoy;

	[Header("Forward force")]
	[SerializeField] protected float m_externalSpeedMultiplier = 1;
	[SerializeField] protected float m_forwardAcceleration;

	[Header("Turning force")]
    [SerializeField] protected Transform FrontTransform;
    [SerializeField] protected Transform rudderBladeTransform;
    [SerializeField] protected Transform rudderTransform;
    [SerializeField] public Interact m_rudderInteractZone;
    protected float rotationAngle = 30;
    [SerializeField] protected float m_turningForce;
    [SerializeField] protected float rotationRudderBlade = 2f;

	[Header("Anchor")]
    [SerializeField] private Transform m_cabestanPivot;
	[SerializeField] protected float m_turnSpeed_1;
	[SerializeField] protected float m_turnSpeed_2;
    [SerializeField] public Interact m_anchorZone;
	[SerializeField] protected float m_brakeImpulse = 30;
	[SerializeField] protected float m_brakeForce = 1;

	[Header("Monitoring")]
	public bool anchorDown = false;
	[SerializeField] protected bool shipSlowingDown = false;
	[SerializeField] protected bool anchorRising = false;
	[SerializeField] protected float m_currentforwardSpeed;
	[SerializeField] public Vector3 m_shipVelocity;
	[SerializeField] public float m_shipVelocityMagnitude;
	#endregion Fields

	#region Privates
	public float forward;
	protected float m_waterLevel;
    public float RudderBladeRotation_Y = 0f;
    protected float RudderRotation_Z = 0f;
	public Rigidbody m_shipRB;
	public SailsManager m_sailsManager;

	#endregion Privates

	protected void Start()
	{
		m_shipRB = GetComponent<Rigidbody>();
		m_sailsManager = GetComponentInChildren<SailsManager>();

		if(!m_ocean && GameObject.FindGameObjectWithTag("Ocean")){
			m_ocean = GameObject.FindGameObjectWithTag("Ocean").GetComponent<Ocean>();
		}
		//m_shipRB.centerOfMass = m_CenterOfMass.position - transform.position;
	}

	//STEER	
	#region Steer
	public void SteerRight(){            
		RudderBladeRotation_Y = rudderBladeTransform.localEulerAngles.y - rotationRudderBlade *Time.deltaTime;

		if (RudderBladeRotation_Y < 360-rotationAngle && RudderBladeRotation_Y > 90f)
		{
			RudderBladeRotation_Y = 360-rotationAngle;
			RudderRotation_Z = 0;
		}
		else{
			RudderRotation_Z = rudderTransform.localEulerAngles.z - (rotationRudderBlade *Time.deltaTime* (360/rotationAngle));
		}

		Vector3 newRotationRudderBlade = new Vector3(0f, RudderBladeRotation_Y, 0f);
		Vector3 newRotationRudder = new Vector3(0f, 0f, RudderRotation_Z);

		rudderBladeTransform.localEulerAngles = newRotationRudderBlade;
		FrontTransform.localEulerAngles = -newRotationRudderBlade;
		rudderTransform.localEulerAngles = newRotationRudder;
	}

	public void SteerLeft(){            
		RudderBladeRotation_Y = rudderBladeTransform.localEulerAngles.y + rotationRudderBlade *Time.deltaTime;

		if (RudderBladeRotation_Y > rotationAngle && RudderBladeRotation_Y < 270f)
		{
			RudderBladeRotation_Y = rotationAngle;
			RudderRotation_Z = 0;
		}
		else{
			RudderRotation_Z = rudderTransform.localEulerAngles.z + (rotationRudderBlade *Time.deltaTime* (360/rotationAngle));
		}

		Vector3 newRotationRudderBlade = new Vector3(0f, RudderBladeRotation_Y, 0f);
		Vector3 newRotationRudder = new Vector3(0f, 0f, RudderRotation_Z);

		rudderBladeTransform.localEulerAngles = newRotationRudderBlade;
		FrontTransform.localEulerAngles = - newRotationRudderBlade;
		rudderTransform.localEulerAngles = newRotationRudder;
	}

	protected void SetRotationRudder(float angle){
		rudderBladeTransform.localEulerAngles = new Vector3(0f, angle, 0f);
		RudderBladeRotation_Y = angle;
	}
	#endregion Steer

	//FORCES
	#region Forces
	protected void AddMainForce(float maxSpeed)
	{
		if(m_currentforwardSpeed < maxSpeed*m_externalSpeedMultiplier && !shipSlowingDown)
		{
			m_currentforwardSpeed += m_forwardAcceleration;
		}		
		else if(m_currentforwardSpeed > maxSpeed*m_externalSpeedMultiplier || shipSlowingDown)
		{
			m_currentforwardSpeed -= m_forwardAcceleration*10;
		}

		if(m_currentforwardSpeed < 0)
			m_currentforwardSpeed = 0;
		Vector3 mainForce = m_CenterOfMass.forward * m_currentforwardSpeed;

		m_shipRB.AddForceAtPosition(mainForce, m_CenterOfMass.position, m_force);
		//
        Debug.DrawRay(transform.position, m_shipVelocity*10, Color.green);
	}

    protected void TurningForce()
    {
        Vector3 turningForceTemp;
		switch (m_sailsManager.m_sailsSate)
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

		float RudderBladeRotNormalized= (RudderBladeRotation_Y > 180) ? RudderBladeRotation_Y - 360 : RudderBladeRotation_Y;
		if((RudderBladeRotNormalized > 5 || RudderBladeRotNormalized < -5) && !anchorDown)
		{
			m_shipRB.AddForceAtPosition(-turningForceTemp*0.5f, FrontTransform.position, m_force);
        	Debug.DrawRay(FrontTransform.position, -turningForceTemp*10, Color.green);
			//
			m_shipRB.AddForceAtPosition(turningForceTemp, rudderBladeTransform.position, m_force);
        	Debug.DrawRay(rudderBladeTransform.position, turningForceTemp*10, Color.green);
		}
    }

	//FLOAT
	protected void ApplyFloatingForce(Buoy point, float coef)
	{
		Vector3 FloatingForce = point.direction * coef * m_floatingCoef;
		m_shipRB.AddForceAtPosition(FloatingForce, point.transform.position, m_force);
		Debug.DrawRay(point.transform.position, FloatingForce, Color.red,0.1f);
	}	
	
	protected void ApplyGravityForce(Buoy point, float coef)
	{
		Vector3 FloatingForce = -point.direction * coef * m_floatingCoef;
		m_shipRB.AddForceAtPosition(FloatingForce, point.transform.position, m_force);
		//Debug.DrawRay(point.transform.position, FloatingForce, Color.yellow,0.5f);
	}

	protected void Float()
	{

		foreach (Buoy point in m_buoy)
		{
			if(point.transform.gameObject.activeInHierarchy){
				if(useOcean && m_ocean){
					if( m_ocean.gameObject.activeInHierarchy )
						m_waterLevel = m_ocean.GetWaterHeightAtLocation(point.transform.position.x, point.transform.position.y);
				}else{
					m_waterLevel = -3;
				}

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
		}
		m_shipVelocity = m_shipRB.velocity;
		m_shipVelocityMagnitude = m_shipRB.velocity.magnitude;

		/*if(m_shipMagnitude > m_maxVelocity){
			m_shipRB.velocity = Vector3.ClampMagnitude(m_shipRB.velocity, m_maxVelocity);
		}*/
	}

	protected IEnumerator Sink(float f = 5){
		
		m_shipRB.drag *= 3;
		m_shipRB.angularDrag *= 3;
		m_buoy[1].transform.gameObject.SetActive(false);

		yield return new WaitForSeconds(2);

		m_buoy[2].transform.gameObject.SetActive(false);
		m_buoy[3].transform.gameObject.SetActive(false);
		m_buoy[4].transform.gameObject.SetActive(false);
		m_buoy[5].transform.gameObject.SetActive(false);

		yield return new WaitForSeconds(5);

		m_buoy[0].transform.gameObject.SetActive(false);
		m_buoy[6].transform.gameObject.SetActive(false);

		yield return new WaitForSeconds(f);

		//m_shipRB.velocity = Vector3.zero;
		//m_shipRB.useGravity = false;
		//m_shipRB.constraints = RigidbodyConstraints.FreezeAll;
	}
	
	//impulse
	protected void AddImpulseAtFloatingPoint(int index, Vector3 direction, float magnitude, ForceMode force){
		m_shipRB.AddForceAtPosition(direction*magnitude, m_buoy[index].transform.position, force);
	}
	#endregion Forces

	//ANCHOR
	#region Anchor
	public bool Anchor(){
		if(shipSlowingDown || anchorRising){
			return false;
		}else{
			if(anchorDown){
				StartCoroutine("RaiseAnchor");
			}else{
				StartCoroutine("LowerAnchor");
			}
			return true;
		}
	}

	protected void TurnCabestan(){
		if(shipSlowingDown){
			m_cabestanPivot.Rotate(0,-m_turnSpeed_1 * Time.deltaTime,0, Space.Self);
		}
		else if (anchorRising){
			m_cabestanPivot.Rotate(0,m_turnSpeed_2 * Time.deltaTime,0, Space.Self);
		}
	}

	protected IEnumerator LowerAnchor(){
		shipSlowingDown = true;

		yield return new WaitForSeconds(3);
		
		/*while(m_shipVelocityMagnitude > 2){
			m_shipRB.velocity -= new Vector3(0,0,m_brakeForce);
			AddImpulseAtFloatingPoint(0, Vector3.down, m_brakeImpulse*m_shipVelocityMagnitude, m_force);
			yield return new WaitForSeconds(0.1f);
		}*/

		AddImpulseAtFloatingPoint(0, Vector3.down, m_brakeImpulse, ForceMode.Impulse);
		m_shipRB.velocity = Vector3.zero;
		m_shipRB.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		anchorDown = true;

		shipSlowingDown = false;
	}

	protected IEnumerator RaiseAnchor(){
		anchorRising = true;

		yield return new WaitForSeconds(3);

		m_shipRB.constraints = RigidbodyConstraints.None;
		anchorDown = false;

		yield return new WaitForSeconds(3);

		anchorRising = false;
	}
	#endregion Anchor
	
	
	//DEBUG
	protected void OnDrawGizmos()
	{
		if(gizmo){
			//Buoy
			foreach (Buoy point in m_buoy)
			{
				if(point.transform.gameObject.activeSelf){
					Gizmos.color = Color.white;
				}
				else{
					Gizmos.color = Color.gray;
				}
				Gizmos.DrawWireSphere(point.transform.position, 1);
				//Vector3 tmp = new Vector3(point.transform.position.x, m_waterLevel ,point.transform.position.z);
				//Gizmos.DrawLine(point.transform.position, tmp);
			}
			
			//Gizmos.color = Color.magenta;
			//if(m_shipRB)
				//Gizmos.DrawWireSphere(m_shipRB.centerOfMass, 0.5f);

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
}