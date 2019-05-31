using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limite : MonoBehaviour
{
    public Animator limite;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.L))
        {
            limite.SetTrigger("Lever");
        }
	}
}
