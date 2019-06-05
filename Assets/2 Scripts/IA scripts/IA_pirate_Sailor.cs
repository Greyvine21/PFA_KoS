using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SailorState{
	Wander = 0,
	Go,
	Repair
}

public class IA_pirate_Sailor : IA_pirate {

	public SailorState m_state = SailorState.Wander;
	
	void Update()
	{
		switch (m_state)
		{
			case SailorState.Wander:
				if(false){

				}else{
					Wander();
				}
			break;
			case SailorState.Go:
				if(false){

				}else{
					
				}
			break;
			case SailorState.Repair:
				if(false){

				}else{
					
				}
			break;
			default:
			break;
		}
	}
}
