using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBehaviour : FloatingShip {

	new void Start()
	{
		base.Start();
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
			AddMainForce(forward);
			TurningForce();
		}
	}

	//INPUT 
    // void UserInput()
    // {
    //     //Forward / reverse
	// 	if (Input.GetKeyDown(KeyCode.UpArrow))
    //     {
	// 		OrderSailsUp();
    //     }		
	// 	if (Input.GetKeyDown(KeyCode.DownArrow))
    //     {
	// 		OrderSailsDown();
    //     }

	// 	float m_inputH = Input.GetAxisRaw("Horizontal");

    //     //Steer left
    //     if (Input.GetKey(KeyCode.LeftArrow) || (m_inputH < 0 && m_rudderInteractZone.isZoneActive()))
    //     {
	// 		SteerLeft();
    //     }

    //     //Steer right
    //     else if (Input.GetKey(KeyCode.RightArrow) || (m_inputH > 0 && m_rudderInteractZone.isZoneActive()))
    //     {
	// 		SteerRight();
    //     }

	// 	if(Input.GetButtonDown("X360_A") && m_anchorZone.isZoneActive()){
	// 		Anchor();
	// 	}
    // }
}
