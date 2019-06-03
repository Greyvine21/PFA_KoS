using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {

	[SerializeField] private Transform m_enemyShip;
	[SerializeField] private Transform m_UIText;
	
	// Update is called once per frame
	void Update () {
		m_UIText.transform.position = Camera.main.WorldToScreenPoint(m_enemyShip.position);
	}
}
