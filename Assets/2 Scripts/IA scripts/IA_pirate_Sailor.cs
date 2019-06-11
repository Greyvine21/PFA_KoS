using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SailorState{
	Wander = 0,
	Go,
	Action
}

public class IA_pirate_Sailor : IA_pirate {

	public SailorState m_state = SailorState.Wander;
	public bool ActionDone;
	void Update()
	{
		switch (m_state)
		{
			case SailorState.Wander:
				if(isGoing){
					m_state = SailorState.Go;
				}else{
					Wander();
				}
			break;
			case SailorState.Go:
				if(CheckDestinationReached()){
					m_state = SailorState.Action;
				}else{
				}
			break;
			case SailorState.Action:
				if(isGoing){
					m_anim.SetBool("Action", false);
					m_state = SailorState.Go;
				}
				else if(ActionDone){
					print("Done");
					ActionDone = false;
					m_anim.SetBool("Action", false);
					m_state = SailorState.Wander;

				}else{
					m_anim.SetBool("Action", true);
				}
			break;
			default:
			break;
		}
	}

}
