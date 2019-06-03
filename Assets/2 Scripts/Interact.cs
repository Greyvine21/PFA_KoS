using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

	public string m_text;
	public Text m_textZone;
	public Transform m_textWorldAnchor;
	public Transform m_snapPoint;
	public bool m_lockPlayer = true;
	public bool m_toggle = true;

	private Animator anim;

	private ActionsController m_player;
	public bool isInteractZoneActive = false;
	public bool playerInZone = false;

	void Start()
	{
		anim = m_textZone.transform.GetComponent<Animator>();
		if(m_textZone != null)
			m_textZone.text = ": " + m_text;
	}

	public bool isZoneActive(){
		return isInteractZoneActive;
	}

	void OnTriggerEnter(Collider other)
	{
		//print(other.name);
		if(other.gameObject.tag == "Player"){
			playerInZone = true;
			anim.SetBool("InteractAnim",true);
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Player"){
			//print("player in area :" + transform.parent.name);
			if(m_player == null)
				m_player = other.gameObject.GetComponent<ActionsController>();

			m_textZone.transform.position = Camera.main.WorldToScreenPoint(m_textWorldAnchor.position);

			//
			if(m_toggle){
				if(!isInteractZoneActive){
					if(m_player.m_isInteracting){
						anim.SetBool("InteractAnim",false);
						m_player.m_isInteracting = false;

						if(m_lockPlayer){
							m_player.m_receiver.m_ControllerAgent.m_agent.enabled = false;
							m_player.m_receiver.m_ControllerAgent.m_agentCanMove =  false;
							m_player.m_receiver.m_ControllerAgent.transform.position =  m_snapPoint.position;
							m_player.m_receiver.m_ControllerAgent.transform.rotation =  m_snapPoint.rotation;
						}

						isInteractZoneActive = true;
					}
				}
				else{
					if(m_lockPlayer){
						if(m_player.m_receiver.m_ControllerAgent.transform.position != m_snapPoint.position){
							m_player.m_receiver.m_ControllerAgent.transform.position =  m_snapPoint.position;
							m_player.m_receiver.m_ControllerAgent.transform.rotation =  m_snapPoint.rotation;
						}
					}

					//
					if(m_player.m_isInteracting){
						anim.SetBool("InteractAnim",true);
						m_player.m_isInteracting = false;

						if(m_lockPlayer){
							m_player.m_receiver.m_ControllerAgent.m_agent.enabled = true;
							m_player.m_receiver.m_ControllerAgent.m_agentCanMove =  true;
						}

						isInteractZoneActive = false;
					}
				}
			}
			else{
				isInteractZoneActive = m_player.m_isInteracting;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Player"){
			anim.SetBool("InteractAnim",false);
			playerInZone = false;
			if(isInteractZoneActive){
				m_player.m_receiver.m_ControllerAgent.m_agent.enabled = true;
				m_player.m_receiver.m_ControllerAgent.m_agentCanMove =  true;
				isInteractZoneActive = false;
			}
		}
	}
}
