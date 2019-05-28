using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct LifeElem
{
    public Slider lifebar;
    public Image ColorBar;
    public float lifePoints;
    public float MaxlifePoints;
}

public class healthManager : MonoBehaviour {


	[Header("Impact zones")]
    [Range(0,100)] public int m_impactPercentage;

    //public GameObject[] m_impactHull;
    public GameObject[] m_impactNavigation;
    public GameObject[] m_impactBridge;
    public GameObject[] m_impactSails;

    // public Slider m_lifebar;
    // public float life = 100;

	[Header("LifeBars")]
    public LifeElem[] m_lifebars;
    public Color Green;
    public Color Orange;
    public Color Red;

	void Start()
	{
        for (int i = 0; i < m_lifebars.Length; i++)
        {
            m_lifebars[i].lifebar.maxValue = m_lifebars[i].MaxlifePoints;
            m_lifebars[i].lifePoints = m_lifebars[i].MaxlifePoints;
            m_lifebars[i].lifebar.value = m_lifebars[i].lifebar.maxValue;
            m_lifebars[i].ColorBar.color = Green;
            //m_lifebars[i].lifePoints = m_lifebars[i].MaxlifePoints;
        }
	}

    public void DecreaseLife(int index, int damages){

        m_lifebars[index].lifePoints -= damages;

        if(m_lifebars[index].lifePoints <= 0){
            if(index == 0){
                if(GetComponent<EnnemyShipBehaviour>())
                    GetComponent<EnnemyShipBehaviour>().Defeat();
                else if(GetComponent<PlayerShipBehaviour>()){
                    m_lifebars[index].lifePoints = m_lifebars[index].MaxlifePoints;
                    //print("Defeat");
                }
            }
        }


        //
        m_lifebars[index].lifebar.value = m_lifebars[index].lifePoints;

        if(m_lifebars[index].lifePoints <=  25f/100f * m_lifebars[index].MaxlifePoints){
            //print("red");
            m_lifebars[index].ColorBar.color = Red;
        }
        else if(m_lifebars[index].lifePoints <= 50f/100f * m_lifebars[index].MaxlifePoints){
            //print("orange");
            m_lifebars[index].ColorBar.color = Orange;
        }
        else{
            //print("green");
            m_lifebars[index].ColorBar.color = Green;
        }
    }

    public void SpawnImpact(GameObject[] Impacts){
        
        if(Random.Range(0, 100) <= m_impactPercentage){
            List<GameObject> unactiveImpacts = new List<GameObject>();
            foreach (GameObject impact in Impacts)
            {
                if(!impact.activeSelf)
                    unactiveImpacts.Add(impact);
            }

            print("impact count : " + unactiveImpacts.Count);
            if(unactiveImpacts.Count > 0)
                unactiveImpacts[Random.Range(0, unactiveImpacts.Count)].SetActive(true);
        }
    }

}
