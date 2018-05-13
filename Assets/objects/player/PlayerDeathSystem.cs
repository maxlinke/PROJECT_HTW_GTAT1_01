using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathSystem : MonoBehaviour {

		[Header("Components")]
	[SerializeField] Rigidbody rb;

		[Header("Prefabs")]
	[SerializeField] GameObject explosionPrefab;

	[HideInInspector] public PlayerModel playerModel;

	bool isExploded;
	bool justActivated;
	bool active;
	Vector3 lastPos;

	void Start () {
		Reset();
	}
	
	void Update () {
		
	}

	void FixedUpdate(){
		if(justActivated){
			Vector3 currentPos = transform.position;
			Vector3 deltaPos = currentPos - lastPos;
			Vector3 velocity = deltaPos / Time.fixedDeltaTime;
			transform.parent = null;
			rb.useGravity = true;
			rb.constraints = RigidbodyConstraints.None;
			rb.velocity = velocity;
			active = true;
		}
		if(active){

		}
		lastPos = transform.position;
	}

	public void Reset(){
		isExploded = false;
		justActivated = false;
		active = false;
	}

	public bool IsExploded(){
		return isExploded;
	}

	//TODO how to go about deactivating this? explode = deactivate?

	public void Explode(){
		justActivated = true;
		//GameObject expl = Instantiate(explosionPrefab);	//TODO explosion pool
		//expl.transform.position = transform.position;		//TODO fragments pool
		isExploded = true;
	}

	public void Crash(){
		justActivated = true;
		isExploded = false;
	}

}
