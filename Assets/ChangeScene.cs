using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
	public int nextScene = 0;
	public int delay = 4;

	public void GoToNextScene(){
		StartCoroutine("delayScene");
	}
	
	private IEnumerator delayScene(){

		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(nextScene);
	}
}
