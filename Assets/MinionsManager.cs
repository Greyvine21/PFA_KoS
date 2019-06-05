﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsManager : MonoBehaviour {
	[SerializeField] public Transform m_Sailors;
    [SerializeField] public Transform m_gunnersLeft;
    [SerializeField] public Transform m_gunnersRight;
	public int nbSailors = 0;
	public IA_pirate_Sailor[] m_sailorsTab;

    private List<IA_pirate_Sailor> availableSailors = new List<IA_pirate_Sailor>();

	// Use this for initialization
	void Start () {
		nbSailors = m_Sailors.childCount;
		m_sailorsTab = new IA_pirate_Sailor[nbSailors];
		
		for (int i = 0; i < nbSailors; i++)
		{
			if(m_Sailors.GetChild(i).GetComponent<AgentController_Receiver>())
				m_sailorsTab[i] = m_Sailors.GetChild(i).GetComponent<AgentController_Receiver>().m_Agent.GetComponent<IA_pirate_Sailor>();
		}
		FindAvailableSailor();
	}
	
	// // Update is called once per frame
	// void Update () 
	// {
		
	// }

	private void FindAvailableSailor(){
		foreach (IA_pirate_Sailor Sailor in m_sailorsTab)
		{
			if(Sailor.m_state == SailorState.Wander)
				availableSailors.Add(Sailor);
		}
	}

	public bool SendUnitTo(Vector3 pos, int nb){
		FindAvailableSailor();
		print("sendUnit");
		if(availableSailors.Count == 0)
				return false;
		
		if(nb > availableSailors.Count){
			for (int i = 0; i <  availableSailors.Count; i++)
			{
				availableSailors[i].GoTo(pos);
			}
			return true;
		}
		else{
			for (int i = 0; i <  nb; i++)
			{
				availableSailors[i].GoTo(pos);
			}
			return true;
		}
	}
}
