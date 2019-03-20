using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerPorte : MonoBehaviour
{
    public GameObject porteDeDroite;
    public GameObject porteDeGauche;

    public float destruction = 1;
    public float construit = 5;

    public Image imageDestruction;
    public Image imageBackground;

    public bool isDestruction = false;

    public float amountDestruction;

    //public GameObject triggerPorte;

    private BoxCollider m_triggerDoor;


	// Use this for initialization
	void Start ()
    {
        m_triggerDoor = gameObject.GetComponent<BoxCollider>();
        imageBackground.enabled = false;
        imageDestruction.enabled = false;
        imageDestruction.fillAmount = destruction;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (destruction >= construit)
        {
            imageBackground.enabled = false;
            imageDestruction.enabled = false;
            porteDeDroite.SetActive(false);
            porteDeGauche.SetActive(false);
            Destroy(m_triggerDoor);
        }
        else if (isDestruction == true)
        {
            imageBackground.enabled = true;
            imageDestruction.enabled = true;
            Destruction(amountDestruction);
        }
        else if (isDestruction == false)
        {
            Destruction(-amountDestruction);
        }
        if (destruction == 0)
        {
            imageBackground.enabled = false;
            imageDestruction.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player") || (other.tag == "Matelots"))
        {
            isDestruction = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player") || (other.tag == "Matelots"))
        {
            isDestruction = false;
        }
    }
    
    public void Destruction (float amount)
    {
        destruction += amount;
        destruction = Mathf.Clamp(destruction, 0, construit);
        imageDestruction.fillAmount = destruction / construit;
    }
}
