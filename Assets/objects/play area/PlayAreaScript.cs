using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class PlayAreaScript : MonoBehaviour {

	void Start () {
		GetComponent<MeshRenderer>().enabled = false;
		this.enabled = false;
	}
	
	void Update () {
		
	}
}
