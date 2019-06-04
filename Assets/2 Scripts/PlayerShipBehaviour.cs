using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBehaviour : FloatingShip {

	private bool isDefeated;
	public bool canMove;
	private CanonManager m_canonsManager;

	new void Start()
	{
		base.Start();
		
		m_canonsManager = GetComponentInChildren<CanonManager>();
		
		m_canonsManager.SetAngleCanonUP(m_canonsManager.m_canonsLeft, 15);
		m_canonsManager.SetAngleCanonUP(m_canonsManager.m_canonsRight, 15);
	}
	
	void Update() 
	{
        //UserInput();
		SailsStateUpdate();
		TurnCabestan();
    }


	void FixedUpdate()
	{
		//float
		Float();

		//Anchor
		if(anchorDown){
			m_shipRB.velocity = Vector3.zero;
		}else{
			if(canMove){
				AddMainForce(forward);
				TurningForce();
			}
		}
	}

	public void Defeat(){
		if(!isDefeated){
			isDefeated = true;
			canMove = false;
			StartCoroutine("Sink");
		}
	}
}
