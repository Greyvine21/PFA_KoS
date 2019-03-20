using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject[] destroy;
    public GameObject explosion;
    public GameObject zone;
	// Use this for initialization
	void Start ()
    {
        //explosion.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Matelots")
        {
            for (int i =0; i<destroy.Length; i++ )
            {
                GameObject effectIns = (GameObject) Instantiate(explosion, zone.transform.position, zone.transform.rotation);
                destroy[i].SetActive(false);
            }
        }
    }
}
