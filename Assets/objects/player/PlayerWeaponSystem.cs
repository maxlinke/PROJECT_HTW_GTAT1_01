using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour {

		[Header("Variables")]
	[SerializeField] float normalFireRate;
	[SerializeField] float increasedFireRate;

	[HideInInspector] public PlayerInput playerInput;
	[HideInInspector] public PlayerModel playerModel;
	[HideInInspector] public PlayerSpecialWeapon specialWeapon;
	[HideInInspector] public BulletPoolScript bulletPool;

	bool useFastBullets;
	bool useIncreasedFireRate;
	float nextShot;

	void Start () {
		specialWeapon.SetWeaponSystem(this);
		Reset();
	}

	void OnEnable(){
		if(specialWeapon != null){
			specialWeapon.enabled = true;
		}
	}

	void OnDisable(){
		specialWeapon.enabled = false;
	}
	
	void Update () {
		//if(Input.GetKeyDown(KeyCode.M))	useFastBullets = !useFastBullets;
		//if(Input.GetKeyDown(KeyCode.N)) useIncreasedFireRate = !useIncreasedFireRate;
		if(playerInput.GetFireInput()){
			if(Time.time > nextShot){
				if(useFastBullets){
					bulletPool.NewFastBullet(transform.position, transform.parent);
				}else{
					bulletPool.NewNormalBullet(transform.position, transform.parent);
				}
				if(useIncreasedFireRate){
					nextShot = Time.time + (1f / increasedFireRate);
				}else{
					nextShot = Time.time + (1f / normalFireRate);
				}
			}
		}
		if(playerInput.GetSpecialInputDown()){
			if(!specialWeapon.IsEmpty()){
				if(specialWeapon.CanFire()){
					specialWeapon.Fire();
				}else{
					playerModel.Shine(new Color(0.5f, 0.5f, 0f));
					float waitTime = specialWeapon.GetTimeToNextFire();
					//Debug.Log("wait " + waitTime);
				}
			}else{
				playerModel.Shine(new Color(0.5f, 0f, 0f));
				//Debug.Log("empty");
			}
		}
	}

	public void Reset(){
		specialWeapon.Reset();
		nextShot = Mathf.NegativeInfinity;
		useFastBullets = true;
		useIncreasedFireRate = true;
	}

	public bool SpecialWeaponCanBeReloaded(){
		return specialWeapon.CanBeReloaded();
	}

	public void ReloadSpecialWeapon(){
		specialWeapon.Reload();
		playerModel.Shine(Color.green);
	}

	public void NotifyWeaponCanBeFiredAgain(){
		playerModel.Shine(Color.cyan);
	}

}
