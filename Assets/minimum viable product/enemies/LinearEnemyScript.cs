using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearEnemyScript : MonoBehaviour, IDamageable {

	[SerializeField] Collider coll;
	[SerializeField] MeshRenderer mr;
	[SerializeField] Color normalColor;
	[SerializeField] Color damageColor;
	[SerializeField] float fallOffTime;
	[SerializeField] int hitpoints;
	[SerializeField] int scoreValue;
	[SerializeField] float moveSpeed;

	[HideInInspector] public float offset;
	[HideInInspector] public GameController gameController;

	MaterialPropertyBlock mpb;
	float lastDamageTime;
	float currentFallOffTime;
	float dissolveVal;
	bool justDied;

	void Start () {
		currentFallOffTime = fallOffTime;
		lastDamageTime = Mathf.NegativeInfinity;
		dissolveVal = 0f;
		justDied = false;
		mpb = new MaterialPropertyBlock();
		mr.GetPropertyBlock(mpb);
	}

	void Update () {
		if(gameController.GetGameOverCompletely()){
			Destroy(this.gameObject);
		}
		if(justDied){
			gameController.Score(scoreValue);
			coll.enabled = false;
			StartCoroutine("DeathStuff");
			justDied = false;
		}
		float lerpVal = Mathf.Clamp01((Time.time - lastDamageTime) / currentFallOffTime);
		mpb.SetColor("_Color", Color.Lerp(damageColor, normalColor, lerpVal));
		mpb.SetFloat("_Dissolve", dissolveVal);
		mr.SetPropertyBlock(mpb);
		transform.localPosition -= new Vector3(0, 0, moveSpeed * Time.deltaTime);
	}

	IEnumerator DeathStuff(){
		while(dissolveVal < 1){
			dissolveVal = Mathf.Clamp01(dissolveVal + 0.15f);
			yield return new WaitForSeconds(0.05f);
		}
		Destroy(this.gameObject);
	}

	public void WeaponDamage(int amount){
		Damage(amount);
	}

	public void CollisionDamage(int amount){
		Damage(amount);
	}

	void Damage(int amount){
		lastDamageTime = Time.time;
		currentFallOffTime = fallOffTime * amount;
		if(((hitpoints - amount) <= 0) && (hitpoints > 0)) justDied = true;
		hitpoints -= amount;
	}

	public void Kill(bool instantly){
		hitpoints = 0;
		lastDamageTime = Time.time;
	}

	void DoCollisionDamage(GameObject otherObject){
		IDamageable damageable = otherObject.GetComponent<IDamageable>();
		if(damageable != null) damageable.CollisionDamage(1);
	}

	void OnCollisionEnter(Collision collision){
		DoCollisionDamage(collision.collider.gameObject);
	}

	void OnCollisionStay(Collision collision){
		DoCollisionDamage(collision.collider.gameObject);
	}

}
