using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

	public string m_text;
	public Text m_textZone;
	public Transform m_snapPoint;

	public Animator anim;

	private ActionsController m_player;
	public bool isInteractZoneActive = false;

	void Start()
	{
		anim = GetComponent<Animator>();
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
			anim.SetBool("InteractControl",true);
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Player"){
			//print("player in area :" + transform.parent.name);
			if(m_player == null)
				m_player = other.gameObject.GetComponent<ActionsController>();
			
			if(!isInteractZoneActive){
				if(m_player.m_isInteracting){
					anim.SetBool("InteractControl",false);
					m_player.m_isInteracting = false;

					m_player.m_receiver.m_ControllerAgent.m_agent.enabled = false;
					m_player.m_receiver.m_ControllerAgent.m_agentCanMove =  false;
					m_player.m_receiver.m_ControllerAgent.transform.position =  m_snapPoint.position;
					m_player.m_receiver.m_ControllerAgent.transform.rotation =  m_snapPoint.rotation;

					isInteractZoneActive = true;
				}
			}
			else{
				if(m_player.m_receiver.m_ControllerAgent.transform.position != m_snapPoint.position){
					m_player.m_receiver.m_ControllerAgent.transform.position =  m_snapPoint.position;
					m_player.m_receiver.m_ControllerAgent.transform.rotation =  m_snapPoint.rotation;
				}

				//
				if(m_player.m_isInteracting){
					anim.SetBool("InteractControl",true);
					m_player.m_isInteracting = false;

					m_player.m_receiver.m_ControllerAgent.m_agent.enabled = true;
					m_player.m_receiver.m_ControllerAgent.m_agentCanMove =  true;
					isInteractZoneActive = false;
				}
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Player"){
			anim.SetBool("InteractControl",false);
			if(isInteractZoneActive){
				m_player.m_receiver.m_ControllerAgent.m_agent.enabled = true;
				m_player.m_receiver.m_ControllerAgent.m_agentCanMove =  true;
				isInteractZoneActive = false;
			}
		}
	}
}
