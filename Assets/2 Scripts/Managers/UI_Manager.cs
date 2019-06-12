using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {
	[SerializeField] private FloatingShip m_PlayerShip;
	[SerializeField] private GameObject m_speed_1;
	[SerializeField] private GameObject m_speed_2;
	[SerializeField] private GameObject m_speed_3;
	[SerializeField] private GameObject m_direction;
	//[SerializeField] private Transform m_enemyShip;
	//[SerializeField] private Transform m_UIText;
	
	// Update is called once per frame
	void Update () {
		//m_UIText.transform.position = Camera.main.WorldToScreenPoint(m_enemyShip.position);

		if(m_PlayerShip.anchorDown){
			m_speed_1.SetActive(false);
			m_speed_2.SetActive(false);
			m_speed_3.SetActive(false);
		}
		else{
			switch (m_PlayerShip.m_sailsManager.m_sailsSate)
			{
				case SailsState.sailsZeroPerCent:
					m_speed_1.SetActive(true);
					m_speed_2.SetActive(false);
					m_speed_3.SetActive(false);
				break;
				
				case SailsState.sailsFiftyPerCent:
					m_speed_1.SetActive(true);
					m_speed_2.SetActive(true);
					m_speed_3.SetActive(false);
				break;
				
				case SailsState.sailsHundredPerCent:
					m_speed_1.SetActive(true);
					m_speed_2.SetActive(true);
					m_speed_3.SetActive(true);
				break;
			}
		}

		m_direction.transform.rotation = Quaternion.Euler(0,0, m_PlayerShip.RudderBladeRotation_Y*2);
	}
}
