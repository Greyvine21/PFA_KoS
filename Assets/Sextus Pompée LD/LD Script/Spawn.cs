using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public GameObject allie;
    public GameObject ennemi;
    public Transform m_matelot;
    public Transform m_ennemiMatelot;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(allie, m_matelot.transform.position, m_matelot.transform.rotation);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Instantiate(ennemi, m_ennemiMatelot.transform.position, m_ennemiMatelot.transform.rotation);
        }
	}
}
