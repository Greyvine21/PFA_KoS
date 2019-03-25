using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct LifeElem
{
    public Slider lifebar;
    public float lifePoints;
    public float MaxlifePoints;
}

public class healthManager : MonoBehaviour {

    public LifeElem[] m_lifebars;
    // public Slider m_lifebar;
    // public float life = 100;

	void Start()
	{
        for (int i = 0; i < m_lifebars.Length; i++)
        {
            m_lifebars[i].lifebar.maxValue = m_lifebars[i].MaxlifePoints;
            m_lifebars[i].lifePoints = m_lifebars[i].MaxlifePoints;
            m_lifebars[i].lifebar.value = m_lifebars[i].lifebar.maxValue;
            //m_lifebars[i].lifePoints = m_lifebars[i].MaxlifePoints;
        }
	}

    public void DecreaseLife(int index, int damages){
        m_lifebars[index].lifePoints -= damages;

        if(m_lifebars[index].lifePoints <= 0){
            m_lifebars[index].lifePoints = m_lifebars[index].MaxlifePoints;
        }

        m_lifebars[index].lifebar.value = m_lifebars[index].lifePoints;
    }

}
