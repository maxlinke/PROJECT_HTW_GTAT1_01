using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour, IDamageable {

		[Header("Components")]
	[SerializeField] PlayerController playerController;

		[Header("Variables")]
	[SerializeField] int maxHitPoints;
	[SerializeField] float invulnerabilityTimeAfterHit;

	[HideInInspector] public PlayerModel playerModel;

	int hitPoints;
	int lastHitPoints;
	float invulnerabilityEnd;
	bool invulnerable;
	bool lastDamageWasCollision;

	void Start () {
		Reset();
	}
	
	void Update () {
		if(invulnerable && (Time.time > invulnerabilityEnd)){
			invulnerable = false;
			playerModel.SetBlinking(false);
		}
		if((hitPoints <= 0) && (hitPoints < lastHitPoints)){
			playerController.InitiateDeath(lastDamageWasCollision);
		}
		lastHitPoints = hitPoints;
	}

	public bool CanBeHealed(){
		return (hitPoints < maxHitPoints);
	}

	public void Heal(){
		hitPoints = maxHitPoints;
		//TODO repair sound, stop the smoke, ...
	}

	public void Reset(){
		hitPoints = maxHitPoints;
		lastHitPoints = hitPoints;
		invulnerable = false;
		invulnerabilityEnd = Mathf.NegativeInfinity;
	}

	public void WeaponDamage(int amount){
		if(!invulnerable){
			lastDamageWasCollision = false;
			Damage(amount);
		}
	}

	public void CollisionDamage(int amount){
		if(!invulnerable){
			lastDamageWasCollision = true;
			Damage(amount);
		}
	}

	public void Kill(bool instantly){
		if(!invulnerable){
			lastDamageWasCollision = instantly;
			Damage(hitPoints);
		}
	}

	void Damage(int amount){
		hitPoints -= amount;
		if((hitPoints == 1) && (hitPoints < lastHitPoints)){
			//TODO clank and start the smoke generator or some shit
			Debug.Log(gameObject.name + " is one hit from death now");
		}
		invulnerable = true;
		invulnerabilityEnd = Time.time + invulnerabilityTimeAfterHit;
		playerModel.SetBlinking(true);
	}

}
