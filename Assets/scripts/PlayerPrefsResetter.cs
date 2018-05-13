using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsResetter : MonoBehaviour {

	void Start () {
		PlayerPrefs.DeleteAll();
		this.gameObject.SetActive(false);
	}
	
	void Update () {
		
	}
}
