using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour {

	// public float RaycastMaxDistance;
	// public LayerMask m_raycastLayers; //19
	// public GameObject m_repairText;

    // [Header("Monitoring")]
	// public bool isSelecting;
	// public Transform m_hitPoint;
	// public string objectSelected;
	// public string TagSelected;
	
	// private Animator m_animRepairText;
	// void Start()
	// {
	// 	//m_repairText.SetActive(false);
	// 	m_animRepairText = m_repairText.GetComponent<Animator>();
	// }

	// void Update () 
	// {
	// 	//RaycastTarget();
	// }

	// private void RaycastTarget()
	// {
	// 	RaycastHit hitInfo;

	// 	if(Physics.Raycast(transform.position + (transform.forward*1), transform.forward, out hitInfo, RaycastMaxDistance, m_raycastLayers, QueryTriggerInteraction.Collide)){
	// 		isSelecting = true;
	// 		print("hit");
	// 		//m_hitPoint.position = hitInfo.point;
	// 		objectSelected = hitInfo.collider.gameObject.name;
	// 		TagSelected = hitInfo.collider.gameObject.tag;

	// 		switch (hitInfo.collider.gameObject.layer)
	// 		{
	// 			case 19:
	// 				m_animRepairText.SetBool("InteractAnim",true);
	// 				//if(!m_repairText.activeSelf) m_repairText.SetActive(true);
	// 				m_repairText.transform.position = Camera.main.WorldToScreenPoint(hitInfo.point);
	// 			break;
	// 			default:
	// 				Debug.LogWarning("Raycast camera");
	// 			break;
	// 		}
	// 	}
	// 	else{
	// 		print("No hit");
	// 		m_animRepairText.SetBool("InteractAnim",false);
	// 		isSelecting = false;
	// 		objectSelected = " ";
	// 		TagSelected = " ";
	// 	}
	// }
}
