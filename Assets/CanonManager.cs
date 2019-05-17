﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonManager : MonoBehaviour {


	[Header("Canons")]
	[SerializeField] public Transform m_canonsRight;
	[SerializeField] public Transform m_canonsLeft;

	[Header("Canons Shoot")]
    [SerializeField] public float m_bulletSpeed;
    [SerializeField] public float m_ForceCanonMultiplier;
    [SerializeField] public GameObject m_canonBall;
    [SerializeField] public AudioClip[] CanonsClips;

	[Header("Canons Reload")]    
    [SerializeField] public float reloadSpeed;
	[SerializeField] public bool StartLoaded = true;
    //[SerializeField] public bool isLoaded;

	[Header("Canons Rotation Horizontal")]
    [SerializeField] public float horizontalRotationSpeed = 0.8f;
    [SerializeField] public float rangeAngleY = 60;   

	[Header("Canons Rotation Vertical")]
    [SerializeField] public float verticalRotationSpeed = 0.8f;
    [SerializeField] public float rangeAngleX = 60;
    [SerializeField] public float offsetX = 0;

	[HideInInspector] public int ActiveCannonsRight, ActiveCannonsLeft;


	void Start()
	{		
		ActiveCannonsRight = 0;
		foreach (Transform child in m_canonsRight)
		{
			if(child.gameObject.activeSelf){
				ActiveCannonsRight ++;
			}
		}
		
		ActiveCannonsLeft = 0;
		foreach (Transform child in m_canonsLeft)
		{
			if(child.gameObject.activeSelf){
				ActiveCannonsLeft ++;
			}
		}
	}

	//shoot
	public bool ShootCanon(Transform m_canonSide){

		foreach (Transform child in m_canonSide)
		{
			//print("OK");
			if(child.GetComponent<Canon>() && child.gameObject.activeSelf){
				if(!child.GetComponent<Canon>().isLoaded){
					//return false;
				}
				child.GetComponent<Canon>().CanonShoot();
			}
		}
		return true;
	}

	//reload
	public bool ReloadCanon(Transform m_canonSide, int nbCanons){

		foreach (Transform child in m_canonSide)
		{
			if(child.GetComponent<Canon>() && child.gameObject.activeSelf){
				if(child.GetComponent<Canon>().isLoaded){
					return false;
				}
				child.GetComponent<Canon>().CanonReload(nbCanons);
			}
		}
		return true;
	}

	//Set angle 
	public void SetAngleCanonUP(Transform m_canonSide, float angle){

		foreach (Transform child in m_canonSide)
		{
			if(child.GetComponent<Canon>() && child.gameObject.activeSelf){
				child.GetComponent<Canon>().SetAngleUp(angle);
			}
		}
	}
	public void SetAngleCanonRIGHT(Transform m_canonSide, float angle){

		foreach (Transform child in m_canonSide)
		{
			if(child.GetComponent<Canon>() && child.gameObject.activeSelf){
				child.GetComponent<Canon>().SetAngleRight(angle);
			}
		}
	}
}
