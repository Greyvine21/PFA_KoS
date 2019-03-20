using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canon : MonoBehaviour {

    [SerializeField] private bool Left;

    [Header("Shooting")]
    [SerializeField] private Transform m_shootPoint;
    [SerializeField] private float m_bulletSpeed;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private bool isLoaded;
    [SerializeField] private GameObject m_canonBall;
    [SerializeField] private bool StartLoaded;


    [Header("UI")]
    [SerializeField] private GameObject m_Bar;

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

	public void CanonReload(bool b){
		if(!isReloading && !isLoaded && (b == Left))
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
			if(Left){
				bullet.GetComponent<ConstantForce>().force = new Vector3(transform.parent.parent.parent.GetComponent<Rigidbody>().velocity.magnitude, 0,0);
			}else{
				bullet.GetComponent<ConstantForce>().force = new Vector3(-transform.parent.parent.parent.GetComponent<Rigidbody>().velocity.magnitude, 0,0);
			}

			isLoaded = false;
			m_Bar.transform.localScale = new Vector3(0,1,1);
		}
	}
}
