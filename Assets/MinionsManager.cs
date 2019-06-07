using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsManager : MonoBehaviour {
	[SerializeField] public Transform m_SailorsRepair;
	[SerializeField] public Transform m_SailorsSails;
    [SerializeField] public Transform m_gunnersLeft;
    [SerializeField] public Transform m_gunnersRight;
	public int nbSailorsRepair = 0;
	public int nbSailorsSails = 0;
	public IA_pirate_Sailor[] m_sailorsRepairTab;
	public IA_pirate_Sailor[] m_sailorsSailsTab;

    //private List<IA_pirate_Sailor> availableSailors = new List<IA_pirate_Sailor>();
    private FloatingShip m_ship;

	// Use this for initialization
	void Start () {
        m_ship = transform.GetComponentInParent<PlayerShipBehaviour>();
		//
		nbSailorsRepair = m_SailorsRepair.childCount;
		m_sailorsRepairTab = new IA_pirate_Sailor[nbSailorsRepair];
		//
		nbSailorsSails = m_SailorsSails.childCount;
		m_sailorsSailsTab = new IA_pirate_Sailor[nbSailorsSails];
		
		for (int i = 0; i < nbSailorsRepair; i++)
		{
			if(m_SailorsRepair.GetChild(i).GetComponent<AgentController_Receiver>())
				m_sailorsRepairTab[i] = m_SailorsRepair.GetChild(i).GetComponent<AgentController_Receiver>().m_Agent.GetComponent<IA_pirate_Sailor>();
		}
		for (int i = 0; i < nbSailorsSails; i++)
		{
			if(m_SailorsSails.GetChild(i).GetComponent<AgentController_Receiver>())
				m_sailorsSailsTab[i] = m_SailorsSails.GetChild(i).GetComponent<AgentController_Receiver>().m_Agent.GetComponent<IA_pirate_Sailor>();
		}
		FindAvailableSailor(m_sailorsSailsTab);
		FindAvailableSailor(m_sailorsRepairTab);
	}
	
	// // Update is called once per frame
	// void Update () 
	// {
		
	// }

	private List<IA_pirate_Sailor> FindAvailableSailor(IA_pirate_Sailor[] tab){

    	List<IA_pirate_Sailor> availableSailors = new List<IA_pirate_Sailor>();

		foreach (IA_pirate_Sailor Sailor in tab)
		{
			if(Sailor.m_state == SailorState.Wander)
				availableSailors.Add(Sailor);
		}
		return availableSailors;
	}

	public bool SendUnitToOnePos(Vector3 pos, IA_pirate_Sailor[] sailorCategoryTab, int nb){
		List<IA_pirate_Sailor> sailorsOK = FindAvailableSailor(sailorCategoryTab);

		if(sailorsOK.Count == 0)
				return false;
		
		if(nb > sailorsOK.Count){
			for (int i = 0; i <  sailorsOK.Count; i++)
			{
				sailorsOK[i].GoTo(pos - m_ship.transform.position);
			}
			return true;
		}
		else{
			for (int i = 0; i <  nb; i++)
			{
				sailorsOK[i].GoTo(pos - m_ship.transform.position);
			}
			return true;
		}
	}

	public bool SendUnitToMultiplePos(Transform[] pos, IA_pirate_Sailor[] sailorCategoryTab, int nb){
		List<IA_pirate_Sailor> sailorsOK = FindAvailableSailor(sailorCategoryTab);

		if(sailorsOK.Count == 0)
				return false;
		
		if(nb > sailorsOK.Count){
			for (int i = 0; i <  sailorsOK.Count; i++)
			{
				sailorsOK[i].GoTo(pos[Random.Range(0, pos.Length)].position - m_ship.transform.position);
			}
			return true;
		}
		else{
			for (int i = 0; i <  nb; i++)
			{
				sailorsOK[i].GoTo(pos[Random.Range(0, pos.Length)].position - m_ship.transform.position);
			}
			return true;
		}
	}

	public IEnumerator SendActionDone(IA_pirate_Sailor[] sailorCategoryTab, float delay = 0){

		yield return new WaitForSeconds(delay);

		foreach (IA_pirate_Sailor sailor in sailorCategoryTab)
		{
			if(sailor.m_state == SailorState.Action)
				sailor.ActionDone = true;
		}
	}

}
