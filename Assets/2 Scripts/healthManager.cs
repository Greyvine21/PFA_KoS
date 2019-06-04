using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*public enum ImpactType
{
	Navigation = 0,
	Cannons,
	Sails
}*/

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
    public int m_ImpactCooldown = 10;

    //public GameObject[] m_impactHull;
    public ImpactZone m_impactNavigation;
    public ImpactZone m_impactBridge;
    public ImpactZone m_impactSails;
    public int m_totalNbImpact;

    // public Slider m_lifebar;
    // public float life = 100;

	[Header("LifeBars")]
    public LifeElem[] m_lifebars;
    public Color Green;
    public Color Orange;
    public Color Red;

	void Start()
	{
        m_totalNbImpact = 0;
        //
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
                if(GetComponentInParent<EnnemyShipBehaviour>())
                    GetComponentInParent<EnnemyShipBehaviour>().Defeat();
                else if(GetComponentInParent<PlayerShipBehaviour>()){
                    //m_lifebars[index].lifePoints = m_lifebars[index].MaxlifePoints;
                    GetComponentInParent<PlayerShipBehaviour>().Defeat();
                }
            }
        }

        refreshUIBar(index);
    }

    public void IncreaseLife(int index, int value){
        m_lifebars[index].lifePoints += value;

        if(m_lifebars[index].lifePoints > m_lifebars[index].MaxlifePoints){
            m_lifebars[index].lifePoints = m_lifebars[index].MaxlifePoints;
        }

        refreshUIBar(index);
    }

    private void refreshUIBar(int index){
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
    
    public void SpawnImpact(ImpactZone impactZone){
        
        if(Random.Range(0, 101) <= m_impactPercentage){
            List<Impact> unactiveImpacts = new List<Impact>();
            foreach (Impact impact in impactZone.zone)
            {
                if(!impact.isActive)
                    unactiveImpacts.Add(impact.GetComponent<Impact>());
            }

            //print("impact count : " + unactiveImpacts.Count);
            if(unactiveImpacts.Count > 0){
                m_totalNbImpact ++;
                if(unactiveImpacts[Random.Range(0, unactiveImpacts.Count)].ActiveImpact()){
                    impactZone.AddImpactUI();
                }
            }
        }
    }

}
