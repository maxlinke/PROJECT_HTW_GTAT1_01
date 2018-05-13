using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

		[Header("Components")]
	[SerializeField] PlayerMovementSystem playerMovementSystem;
	[SerializeField] PlayerWeaponSystem playerWeaponSystem;
	[SerializeField] PlayerHealthSystem playerHealthSystem;
	[SerializeField] PlayerDeathSystem playerDeathSystem;

		[Header("Plane Prefabs")]
	[SerializeField] GameObject spectrePlanePrefab;
	[SerializeField] GameObject waspPlanePrefab;
	[SerializeField] GameObject griffonPlanePrefab;
	[SerializeField] GameObject razorbackPlanePrefab;
	[SerializeField] GameObject arrowheadPlanePrefab;

		[Header("SPW Prefabs")]
	[SerializeField] GameObject spectreSPWPrefab;
	[SerializeField] GameObject waspSPWPrefab;
	[SerializeField] GameObject griffonSPWPrefab;
	[SerializeField] GameObject razorheadSPWPrefab;
	[SerializeField] GameObject arrowheadSPWPrefab;

	PlayerModel playerModel;
	GameController gameController;
	int playerNumber;
	int maxLives;
	int lives;

	//TODO initialize and reinitialize? what to do on player death. respawning via new instantiate sounds dumb, so i'd much rather just reinit this guy
	//TODO that would also make level-reloading very quick. just re-initialize everything instead of instantiating it all again
	//		... so the level load is only triggered once and loads all the shit
	//		... and from then on its the gamecontroller that manages stuff
	//TODO reinitializing just calls the other systems back up
	//TODO when to use the invincible layer? probably when "spawning" (enemies and player alike), so that you aren't registered as a target then

	void Start(){
		
	}

	public void Initialize(InputType inputType, PlaneType planeType){	//TODO and weapons and such
		PlayerInput playerInput = PlayerInput.Get(inputType);
		playerMovementSystem.playerInput = playerInput;
		playerWeaponSystem.playerInput = playerInput;
		GameObject modelObject;
		GameObject SPWObject;
		switch(planeType){
		case PlaneType.SPECTRE:
			modelObject = InstantiatePrefabAsChild(spectrePlanePrefab);
			SPWObject = InstantiatePrefabAsChild(spectreSPWPrefab);
			break;
		case PlaneType.WASP:
			modelObject = InstantiatePrefabAsChild(waspPlanePrefab);
			SPWObject = InstantiatePrefabAsChild(waspSPWPrefab);
			break;
		case PlaneType.GRIFFON:
			modelObject = InstantiatePrefabAsChild(griffonPlanePrefab);
			SPWObject = InstantiatePrefabAsChild(griffonSPWPrefab);
			break;
		case PlaneType.RAZORBACK:
			modelObject = InstantiatePrefabAsChild(razorbackPlanePrefab);
			SPWObject = InstantiatePrefabAsChild(razorbackPlanePrefab);
			break;
		case PlaneType.ARROWHEAD:
			modelObject = InstantiatePrefabAsChild(arrowheadPlanePrefab);
			SPWObject = InstantiatePrefabAsChild(arrowheadSPWPrefab);
			break;
		default:
			throw new UnityException("unknown plane type");
		}
		playerModel = modelObject.GetComponent<PlayerModel>();
		playerMovementSystem.playerModel = playerModel;
		playerWeaponSystem.playerModel = playerModel;
		playerHealthSystem.playerModel = playerModel;
		playerDeathSystem.playerModel = playerModel;
		PlayerSpecialWeapon playerSPW = SPWObject.GetComponent<PlayerSpecialWeapon>();
		playerWeaponSystem.specialWeapon = playerSPW;
	}

	public void LevelResetInit(){
		lives = maxLives;
		ResetAllComponents();
	}

	public void SetFurtherInitData(int playerNumber, int maxLives, GameController gameController, BulletPoolScript playerBulletPool, BoxCollider playAreaCollider){
		this.playerNumber = playerNumber;
		this.maxLives = maxLives;
		this.lives = maxLives;
		this.gameController = gameController;
		playerWeaponSystem.bulletPool = playerBulletPool;
		playerMovementSystem.playAreaCollider = playAreaCollider;
	}

	public void SetRegularComponentsActive(bool value){
		playerMovementSystem.enabled = value;
		playerHealthSystem.enabled = value;
		playerWeaponSystem.enabled = value;
	}

	public void InitiateRespawn(){
		if(!playerDeathSystem.IsExploded()){
			playerDeathSystem.Explode();
		}
		SetRegularComponentsActive(false);
		ResetAllComponents();
		playerModel.SetBlinking(true);
	}

	//TODO dont forget layer management.. dead = background... alive = foreground

	public void FinishRespawn(){
		SetRegularComponentsActive(true);
		playerModel.SetBlinking(false);
	}

	public void InitiateDeath(bool explode){
		/*
		lives -= 1;
		if(explode){
			playerDeathSystem.Explode();
			//TODO explosion, deactivating the model's mesh renderers
			//TODO or give the responsibility of all of this to a deathsystem
		}else{
			playerDeathSystem.Crash();
		}
		*/
		//TODO move to background layer
		gameController.GameOver();
		//Debug.Log(gameObject.name + " is ded");
	}

	void ResetAllComponents(){
		playerModel.Reset();
		playerMovementSystem.Reset();
		playerWeaponSystem.Reset();
		playerHealthSystem.Reset();
		playerDeathSystem.Reset();
	}

	GameObject InstantiatePrefabAsChild(GameObject prefab){
		GameObject instantiated = Instantiate(prefab, Vector3.zero, Quaternion.identity, this.transform) as GameObject;
		instantiated.transform.localPosition = Vector3.zero;
		instantiated.transform.localRotation = Quaternion.identity;
		return instantiated;
	}

	void OnCollisionEnter(Collision collision){
		//Debug.Log("collision " + Time.time);	//TODO notify the other components here? i guess?
		//if(collision.collider.gameObject.layer == LayerMask.NameToLayer("asdf")){
		//	playerHealthSystem.Kill(true);
		//}
	}

}
