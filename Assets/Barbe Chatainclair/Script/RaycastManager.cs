using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour {

	public float RaycastMaxDistance;
	public LayerMask m_raycastLayers;
	public Transform m_hitPoint;
	public bool isSelecting;
	public string objectSelected;
	public string TagSelected;
	
	
	void Update () {
		RaycastTarget();
	}

	private void RaycastTarget()
	{
		RaycastHit hitInfo;

		if(Physics.Raycast(transform.position + (transform.forward*1), transform.forward, out hitInfo, RaycastMaxDistance, m_raycastLayers, QueryTriggerInteraction.Collide)){
			isSelecting = true;
			m_hitPoint.position = hitInfo.point;
			objectSelected = hitInfo.collider.gameObject.name;
			TagSelected = hitInfo.collider.gameObject.tag;
		}
		else{
			isSelecting = false;
			objectSelected = " ";
			TagSelected = " ";
		}
	}
}
