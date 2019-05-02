using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Canon : MonoBehaviour {
	
    [SerializeField] public bool m_Left;

    [Header("Shooting")]
    [SerializeField] public Transform[] m_shootPoint;
    //[SerializeField] private float m_bulletSpeed;
    //[SerializeField] private GameObject m_canonBall;

    [Header("Reloading")]
    [SerializeField] private bool StartLoaded;
    //[SerializeField] private float reloadSpeed;
    [SerializeField] public bool isLoaded;

    [Header("UI")]
    [SerializeField] private GameObject m_Bar;

	private bool isReloading;

	private FloatingShip m_ship;
	private AudioSource m_source;

	void Start()
	{
		m_ship = transform.GetComponentInParent<FloatingShip>();
		m_source = GetComponent<AudioSource>();

		if(StartLoaded){
			isLoaded = true;
			if(m_Bar != null)
				m_Bar.transform.localScale = new Vector3(1,1,1);
		}else{
			isLoaded = false;
			if(m_Bar != null)
				m_Bar.transform.localScale = new Vector3(0,1,1);
		}
	}


	public void CanonReload(int nbCannons){
		if(!isReloading && !isLoaded)
			StartCoroutine("ReloadCor", nbCannons);
	}


	private IEnumerator ReloadCor(int nbCannons){
		isReloading = true;

		m_Bar.transform.localScale = new Vector3(0,1,1);
		while(m_Bar.transform.localScale.x < 1){
			m_Bar.transform.localScale += new Vector3(0.01f,0,0);
			yield return new WaitForSeconds(1/(m_ship.reloadSpeed/nbCannons));
		}

		m_Bar.transform.localScale = new Vector3(1,1,1);

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
			m_Bar.transform.localScale = new Vector3(0,1,1);
		}
	}

	private IEnumerator DelayShoot(Transform point){
		//Delay
		yield return new WaitForSeconds(Random.Range(0, 0.5f));

		//Spawn bullet
		GameObject bullet = Instantiate(m_ship.m_canonBall, point.position, point.rotation, transform);
		bullet.name = "CanonBall";

		//Sound
		if(m_source != null)
			m_source.PlayOneShot(m_ship.CanonsClips[Random.Range(0, m_ship.CanonsClips.Length)]);

		//Play FX
		if(point.GetChild(0).GetComponent<ParticleSystem>() != null)
			point.GetChild(0).GetComponent<ParticleSystem>().Play(true);

		//Add main force in Bullet
		bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * m_ship.m_bulletSpeed, ForceMode.Impulse);


		//Add secondary force in Bullet and Recoil		
		
		//transform.GetComponentInParent<Rigidbody>().AddForceAtPosition(-bullet.transform.forward * m_ship.m_bulletSpeed * m_ship.m_ForceCanonMultiplier, transform.position, ForceMode.Acceleration);
		if(m_Left){			
			transform.GetComponentInParent<Rigidbody>().AddTorque(-GetComponentInParent<FloatingShip>().transform.forward * m_ship.m_bulletSpeed * m_ship.m_ForceCanonMultiplier, ForceMode.Acceleration);

			//bullet.GetComponent<ConstantForce>().force = new Vector3(transform.GetComponentInParent<Rigidbody>().velocity.magnitude, 0,0);
		}else{
			transform.GetComponentInParent<Rigidbody>().AddTorque(GetComponentInParent<FloatingShip>().transform.forward * m_ship.m_bulletSpeed * m_ship.m_ForceCanonMultiplier, ForceMode.Acceleration);

			//bullet.GetComponent<ConstantForce>().force = new Vector3(-transform.GetComponentInParent<Rigidbody>().velocity.magnitude, 0,0);
		}
	}
}
