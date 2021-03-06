﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyScript : MonoBehaviour, IDamageable {

	[SerializeField] MeshRenderer mr;
	[SerializeField] Color normalColor;
	[SerializeField] Color damageColor;
	[SerializeField] float fallOffTime;
	[SerializeField] float oscillateTime;
	[SerializeField] float oscillateWidth;

	MaterialPropertyBlock mpb;
	float lastDamageTime;
	float currentFallOffTime;

	void Start () {
		currentFallOffTime = fallOffTime;
		lastDamageTime = Mathf.NegativeInfinity;
		mpb = new MaterialPropertyBlock();
		mr.GetPropertyBlock(mpb);
	}
	
	void Update () {
		float lerpVal = Mathf.Clamp01((Time.time - lastDamageTime) / currentFallOffTime);
		mpb.SetColor("_Color", Color.Lerp(damageColor, normalColor, lerpVal));
		mr.SetPropertyBlock(mpb);
		transform.localPosition = new Vector3(Mathf.Sin(Mathf.PI * 2f * Time.time / oscillateTime) * oscillateWidth, 0, transform.localPosition.z);;
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
	}

	public void Kill(bool instantly){
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
