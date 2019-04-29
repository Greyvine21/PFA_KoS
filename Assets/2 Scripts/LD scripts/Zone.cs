using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zone : MonoBehaviour
{
    public float maxCaptureZone = 100;
    public float captureZone = 1;
    public float amountZone = 1; 
    public Image captureBarZone;
    public bool isCapture = false;
    public GameObject Victory;
    public LayerMask Captureur;

    private int allieInside = 0;
    private int ennemiInside = 0;

	// Use this for initialization
	void Start ()
    {
        captureBarZone.fillAmount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Matelots")
        {
            allieInside++;
        }
        if (other.tag == "Ennemi")
        {
            ennemiInside++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Matelots")
        {
            allieInside--;
        }
        if (other.tag == "Ennemi")
        {
            ennemiInside--;
        }
    }

    private void Update()
    {
        //print(allieInside);
        /*if(allieInside >= 1)
        {
            //Debug.Log("wesh");
            CaptureZoneIncrease(amountZone);
        }*/

        if((ennemiInside == allieInside) && (ennemiInside > 0) && (allieInside > 0))
        {
            CaptureZoneIncrease(0);
        }
        else if (allieInside > ennemiInside)
        {
            CaptureZoneIncrease(amountZone);
        }
        else if(ennemiInside>allieInside)
        {
            CaptureZoneIncrease(-amountZone);
        }
        else
        {
            CaptureZoneIncrease(-amountZone / 2);
        }
    }

    public void CaptureZoneIncrease (float amount)
    {
        captureZone += amount;
        captureZone = Mathf.Clamp(captureZone, 0, maxCaptureZone);
        captureBarZone.fillAmount = captureZone / maxCaptureZone;

        if(captureZone == maxCaptureZone && isCapture == false)
        {
            Debug.Log("Zone 1 capturé");
            isCapture = true;
            Victory.SendMessage("Captured", 1);
        }
    }
}
