using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour {

	[SerializeField] Transform leftBound;
	[SerializeField] Transform rightBound;
	[SerializeField] float spawnRateIncreaseTime;

	[Range(0f, 1f)]
	[SerializeField] float chanceForOscillatingEnemy;

	[SerializeField] GameObject oscillatingEnemyPrefab;
	[SerializeField] GameObject linearEnemyPrefab;

	[HideInInspector] public GameController gameController;

	float startTime;
	float nextSpawn;

	void OnEnable(){
		startTime = Time.time;
		nextSpawn = Time.time;
	}

	void Start(){
		
	}
	
	void Update(){
		if(Time.time > nextSpawn){
			SpawnNextEnemy();
		}
	}

	void SpawnNextEnemy(){
		if(Random.value < chanceForOscillatingEnemy){
			GameObject osc = Instantiate(oscillatingEnemyPrefab) as GameObject;
			osc.transform.parent = this.transform.parent;
			osc.transform.localPosition = this.transform.localPosition;
			osc.transform.localRotation = Quaternion.identity;
			OscillatingEnemyScript oes = osc.GetComponent<OscillatingEnemyScript>();
			oes.offset = Random.value * Mathf.PI * 2f;
			oes.gameController = gameController;
		}else{
			GameObject lin = Instantiate(linearEnemyPrefab) as GameObject;
			lin.transform.parent = this.transform.parent;
			lin.transform.localPosition = this.transform.localPosition + Vector3.Lerp(leftBound.localPosition, rightBound.localPosition, Random.value);
			lin.transform.localRotation = Quaternion.identity;
			LinearEnemyScript les = lin.GetComponent<LinearEnemyScript>();
			les.gameController = gameController;
		}
		float spawnRate = 1f + ((Time.time - startTime) / spawnRateIncreaseTime);
		nextSpawn = Time.time + (2f / spawnRate);
	}

}
