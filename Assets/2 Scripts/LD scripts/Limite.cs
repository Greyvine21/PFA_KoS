using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limite : MonoBehaviour
{
    public Animator limite;

    public int startCounter = 0;
    
	void Start ()
    {
        StartCoroutine("TheLimit");
	}

    IEnumerator TheLimit ()
    {
        yield return new WaitForSeconds(startCounter);
        limite.SetTrigger("Lever");
    }
}
