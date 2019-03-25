using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canon : MonoBehaviour {

    [SerializeField] public bool m_Left;

    [Header("Shooting")]
    [SerializeField] private Transform m_shootPoint;
    [SerializeField] private float m_bulletSpeed;
    [SerializeField] private float reloadSpeed;
    [SerializeField] public bool isLoaded;
    [SerializeField] private GameObject m_canonBall;
    [SerializeField] private bool StartLoaded;

    [Header("aiming Canon")]
    [SerializeField] private Interact CanonZone;
    [SerializeField] private Transform CanonPivotTransform;
    [SerializeField] private float rotationCanon = 2f;


    [Header("UI")]
    [SerializeField] private GameObject m_Bar;

    private float CanonRotation_X = 0f;
     private float rotationAngle = 30;

	private bool isReloading;

	void Start()
	{
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

		float m_inputV = Input.GetAxisRaw("Vertical");

		//move Down
		if (Input.GetKey(KeyCode.LeftArrow) || (m_inputV > 0 && CanonZone.isZoneActive()))
        {
            CanonRotation_X = CanonPivotTransform.localEulerAngles.x + rotationCanon;

            if (CanonRotation_X > rotationAngle && CanonRotation_X < 270f)
            {
                CanonRotation_X = rotationAngle;
            }
			// else{
			// 	RudderRotation_Z = rudderTransform.localEulerAngles.z + (rotationRudderBlade * (360/rotationAngle));
			// }

            Vector3 newRotationRudderBlade = new Vector3(CanonRotation_X, 180, 0f);
            //Vector3 newRotationRudder = new Vector3(0f, 0f, RudderRotation_Z);

            CanonPivotTransform.localEulerAngles = newRotationRudderBlade;
			//rudderTransform.localEulerAngles = newRotationRudder;
        }

        //move Up
        else if (Input.GetKey(KeyCode.RightArrow) || (m_inputV < 0 && CanonZone.isZoneActive()))
        {
            CanonRotation_X = CanonPivotTransform.localEulerAngles.x - rotationCanon;

            if (CanonRotation_X < 360-rotationAngle && CanonRotation_X > 90f)
            {
                CanonRotation_X = 360-rotationAngle;
            }
			// else{
			// 	RudderRotation_Z = rudderTransform.localEulerAngles.z - (rotationRudderBlade * (360/rotationAngle));
			// }

            Vector3 newRotationRudderBlade = new Vector3(CanonRotation_X, 180, 0f);
            //Vector3 newRotationRudder = new Vector3(0f, 0f, RudderRotation_Z);

            CanonPivotTransform.localEulerAngles = newRotationRudderBlade;
			//rudderTransform.localEulerAngles = newRotationRudder;
        }
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
			//print(gameObject.name + " shoot");
			GameObject bullet = Instantiate(m_canonBall, m_shootPoint.position, m_shootPoint.rotation);
			bullet.name = "CanonBall";
			bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * m_bulletSpeed, ForceMode.Impulse);
			if(m_Left){
				//bullet.GetComponent<ConstantForce>().force = new Vector3(transform.parent.parent.parent.GetComponent<Rigidbody>().velocity.magnitude, 0,0);
				bullet.GetComponent<ConstantForce>().force = new Vector3(transform.GetComponentInParent<Rigidbody>().velocity.magnitude, 0,0);
			}else{
				bullet.GetComponent<ConstantForce>().force = new Vector3(-transform.GetComponentInParent<Rigidbody>().velocity.magnitude, 0,0);
			}

			isLoaded = false;
			m_Bar.transform.localScale = new Vector3(0,1,1);
		}
	}
}
