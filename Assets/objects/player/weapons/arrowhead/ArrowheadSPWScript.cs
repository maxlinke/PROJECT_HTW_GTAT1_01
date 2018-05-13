using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowheadSPWScript : PlayerSpecialWeapon {

	[SerializeField] float fireInterval;
	[SerializeField] GameObject deathSpherePrefab;

	PlayerWeaponSystem pws;
	float nextFire;
	bool couldFire;

	void Start(){
		Reset();
	}

	void Update(){
		if(CanFire() && !couldFire){
			pws.NotifyWeaponCanBeFiredAgain();
		}
		couldFire = CanFire();
	}

	public override void SetWeaponSystem(PlayerWeaponSystem pws){
		this.pws = pws;
	}

	public override void Fire(){
		if(CanFire()){
			nextFire = Time.time + fireInterval;
			GameObject deathSphere = Instantiate(deathSpherePrefab) as GameObject;
			deathSphere.transform.parent = this.transform.parent.parent;
			deathSphere.transform.localPosition = this.transform.parent.localPosition;
		}
	}

	public override void Reload(){
		
	}

	public override bool IsEmpty(){
		return false;
	}

	public override bool CanFire(){
		return (!IsEmpty() && (Time.time > nextFire));
	}

	public override float GetTimeToNextFire(){
		return Mathf.Clamp((nextFire - Time.time), 0, float.PositiveInfinity);
	}

	public override bool CanBeReloaded(){
		return false;
	}

	public override int GetAmmoCount(){
		return int.MaxValue;
	}

	public override void Reset(){
		nextFire = Mathf.NegativeInfinity;
	}
}
