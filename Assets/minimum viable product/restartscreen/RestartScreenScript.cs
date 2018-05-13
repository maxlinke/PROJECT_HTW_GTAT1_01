using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartScreenScript : MonoBehaviour {

	[SerializeField] KeyCode restartKey;
	[SerializeField] float blinkInterval;
	[SerializeField] Text textField;
	float nextBlink;

	[HideInInspector] public GameController gameController;

	void Start(){
		textField.text = "Press " + restartKey + " to start";
		nextBlink = Time.time + blinkInterval;
	}

	void Update(){
		if(Time.time > nextBlink){
			textField.enabled = !textField.enabled;
			nextBlink = Time.time + blinkInterval;
		}
		if(Input.GetKeyDown(restartKey)){
			gameController.RestartGame();
			textField.text = "Press " + restartKey + " to restart";
			this.gameObject.SetActive(false);
		}
	}
}
