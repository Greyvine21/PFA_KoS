using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    //public GameObject[] zone;
    public float maxCaptureZoneVictory = 100;
    public float capture = 1;
    public float amount = 1; 
    public Image captureBarVictory;
    public bool isVictory = false;
    private int numZone = 0;
    

    // Use this for initialization
    void Start()
    {
        captureBarVictory.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isVictory == true)
        {
            Debug.Log("Victory");
            CaptureZoneVictory(amount);
        }
    }
    public void CaptureZoneVictory(float amount)
    {
        capture += amount;
        capture = Mathf.Clamp(capture, 0, maxCaptureZoneVictory);
        captureBarVictory.fillAmount = capture / maxCaptureZoneVictory;
    }
    public void Captured ()
    {
        numZone++;
        if (numZone == 3)
        {
            isVictory = true;
        }
    }
}
