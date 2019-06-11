using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactZone : MonoBehaviour {

    [Range(0,100)] public int m_impactPercentage;
    public Impact[] zone;
    public GameObject[] ImpactUI;
    public int activeImpactNb;

	void Start()
	{
		zone = new Impact[transform.childCount];

		for (int i = 0; i < zone.Length; i++)
		{
			zone[i] = transform.GetChild(i).GetComponent<Impact>();
		}

		Reset();
	}

    public void Reset(){
        foreach (Impact item in zone)
        {
            item.DisableImpact(false);
        }

        foreach (GameObject item in ImpactUI)
        {
            item.SetActive(false);
        }
    }

    public void AddImpactUI(){
        for (int i = 0; i < ImpactUI.Length; i++)
        {
            if(!ImpactUI[i].activeInHierarchy){
                ImpactUI[i].SetActive(true);
                return;
            }
        }
    }

    public bool RemoveImpactUI(){
        for (int i = ImpactUI.Length-1; i >= 0; i--)
        {
            if(ImpactUI[i].activeInHierarchy){
                ImpactUI[i].SetActive(false);
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmos()
    {
        /*Gizmos.color = Color.magenta;
        foreach (Impact item in zone)
        {
            Gizmos.DrawSphere( item.transform.position - transform.GetComponentInParent<FloatingShip>().transform.position, 3);
        }*/
    }
}
