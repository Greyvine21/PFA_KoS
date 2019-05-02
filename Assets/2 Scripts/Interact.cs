using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

	public string m_text;
	public Text m_textZone;
	public Transform m_snapPoint;

	private Animator anim;

	private Controller3D m_player;
	public bool isInteractZoneActive = false;

	void Start()
	{
		anim = GetComponent<Animator>();
		m_textZone.text = ": " + m_text;
	}

	public bool isZoneActive(){
		return isInteractZoneActive;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player"){
			anim.SetBool("InteractControl",true);
		}
	}

	void OnTriggerStay(Collider other)
	{
		print(other.name);
		if(other.gameObject.tag == "Player"){
			//print("player in area :" + transform.parent.name);
			if(m_player == null)
				m_player = other.gameObject.GetComponent<Controller3D>();
			
			if(!isInteractZoneActive){
				if(m_player.m_isInteracting){
					anim.SetBool("InteractControl",false);
					m_player.LockPlayerOnPoint(m_snapPoint);
					isInteractZoneActive = true;
				}
			}
			else{
				if(m_player.transform.position != m_snapPoint.position){
					m_player.transform.position = m_snapPoint.position;
					m_player.transform.rotation = m_snapPoint.rotation;
				}

				//
				if(m_player.m_isInteracting){
					anim.SetBool("InteractControl",true);
					m_player.UnlockLockPlayerFromPoint(m_snapPoint);
					isInteractZoneActive = false;
				}
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Player"){
			anim.SetBool("InteractControl",false);

			m_player.UnfixPlayer();
			isInteractZoneActive = false;
			m_player.canMove = true;
		}
	}
}
