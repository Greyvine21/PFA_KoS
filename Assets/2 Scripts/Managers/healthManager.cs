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
    public Slider bar;
    public Image ColorBar;
    public float lifePoints;
    public float MaxlifePoints;
}

public class healthManager : MonoBehaviour
{


    public float m_regenDelay = 1;
    public int m_regenAmount = 1;
    public bool canRegen;

    [Header("Impact zones")]
    public bool m_useImpact = true;
    public int m_ImpactCooldown = 10;

    //public GameObject[] m_impactHull;
    public ImpactZone m_impactNavigation;
    public ImpactZone m_impactBridge;
    public ImpactZone m_impactSails;
    public int m_totalNbImpact;

    // public Slider m_lifebar;
    // public float life = 100;

    [Header("LifeBars")]
    public LifeElem m_lifebar;
    public Color Green;
    public Color Orange;
    public Color Red;

    public delegate void LifeReachZero(object sender);
    public event LifeReachZero OnLifeReachZero;
    public void CallOnLifeReachZero()
    {
        if (OnLifeReachZero != null)
        {
            OnLifeReachZero(this);
        }
    }

    private bool isRegen;
    void Start()
    {
        m_totalNbImpact = 0;
        //
        m_lifebar.bar.maxValue = m_lifebar.MaxlifePoints;
        m_lifebar.lifePoints = m_lifebar.MaxlifePoints;
        m_lifebar.bar.value = m_lifebar.bar.maxValue;
        m_lifebar.ColorBar.color = Green;
        //m_lifebars[i].lifePoints = m_lifebars[i].MaxlifePoints;
    }

    public void DecreaseLife(int damages)
    {

        m_lifebar.lifePoints -= damages;

        if (m_lifebar.lifePoints <= 0)
        {
            CallOnLifeReachZero();
            /*if(GetComponentInParent<EnnemyShipBehaviour>())
                GetComponentInParent<EnnemyShipBehaviour>().Defeat();
            else if(GetComponentInParent<PlayerShipBehaviour>()){
                GetComponentInParent<PlayerShipBehaviour>().Defeat();
            }*/
        }

        refreshUIBar();
    }

    public void IncreaseLife(int value)
    {
        m_lifebar.lifePoints += value;

        if (m_lifebar.lifePoints > m_lifebar.MaxlifePoints)
        {
            m_lifebar.lifePoints = m_lifebar.MaxlifePoints;
        }

        refreshUIBar();
    }

    private void refreshUIBar()
    {
        m_lifebar.bar.value = m_lifebar.lifePoints;

        if (m_lifebar.lifePoints <= 25f / 100f * m_lifebar.MaxlifePoints)
        {
            //print("red");
            m_lifebar.ColorBar.color = Red;
        }
        else if (m_lifebar.lifePoints <= 50f / 100f * m_lifebar.MaxlifePoints)
        {
            //print("orange");
            m_lifebar.ColorBar.color = Orange;
        }
        else
        {
            //print("green");
            m_lifebar.ColorBar.color = Green;
        }
    }

    public void SpawnImpact(ImpactZone impactZone)
    {

        if (Random.Range(0, 101) <= impactZone.m_impactPercentage)
        {
            List<Impact> unactiveImpacts = new List<Impact>();
            foreach (Impact impact in impactZone.zone)
            {
                if (!impact.isActive)
                    unactiveImpacts.Add(impact.GetComponent<Impact>());
            }

            //print("impact count : " + unactiveImpacts.Count);
            if (unactiveImpacts.Count > 0)
            {
                m_totalNbImpact++;
                if (unactiveImpacts[Random.Range(0, unactiveImpacts.Count)].ActiveImpact())
                {
                    impactZone.AddImpactUI();
                }
            }
        }
    }

    public void Regen()
    {
        if (!isRegen && m_totalNbImpact <= 0 && m_lifebar.lifePoints < m_lifebar.MaxlifePoints)
        {
            StartCoroutine("RegenCor");
        }
    }
    public IEnumerator RegenCor()
    {
        isRegen = true;
        yield return new WaitForSeconds(m_regenDelay);
        IncreaseLife(m_regenAmount);
        isRegen = false;
    }

}