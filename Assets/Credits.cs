using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

	public Transform creditTxt;
	public float speed1 = 50;
	public float speed2 = 80;
	
	private float finalSpeed;
	
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		
		Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("X360_A")){
			finalSpeed = speed2;
		}else{
			finalSpeed = speed1;
		}

		creditTxt.Translate(0,finalSpeed*Time.deltaTime,0, Space.Self);

		//
		if(Input.GetButtonDown("X360_B") || Input.GetButtonUp("X360_Start") || Input.GetKeyDown(KeyCode.Escape)|| creditTxt.localPosition.y > 0 ){
        	SceneManager.LoadScene(0);
		}
	}
}
