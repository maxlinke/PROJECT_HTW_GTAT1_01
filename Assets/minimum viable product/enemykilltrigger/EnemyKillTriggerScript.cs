using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillTriggerScript : MonoBehaviour {

	void Start(){
		
	}
	
	void Update(){
		
	}

	void OnTriggerEnter(Collider otherCollider){
		//Debug.Log("destroying " + otherCollider.gameObject.name);
		Destroy(otherCollider.gameObject);
	}

}
