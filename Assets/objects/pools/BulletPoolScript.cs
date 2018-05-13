using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolScript : MonoBehaviour {

	[SerializeField] int poolSize;
	[SerializeField] GameObject normalBulletPrefab;
	[SerializeField] GameObject fastBulletPrefab;

	List<GameObject> normalPool;
	List<GameObject> fastPool;

	void Start () {
		normalPool = new List<GameObject>();
		fastPool = new List<GameObject>();
	}
	
	void Update () {
		
	}

	public void NewNormalBullet(Vector3 position, Transform parent){
		GameObject bullet = TryGetFromPool(normalPool);
		if(bullet == null) bullet = InstantiateBullet(normalBulletPrefab, parent);
		bullet.transform.position = position;
		bullet.transform.localRotation = Quaternion.identity;
		bullet.SetActive(true);
	}

	public void NewFastBullet(Vector3 position, Transform parent){
		GameObject bullet = TryGetFromPool(fastPool);
		if(bullet == null) bullet = InstantiateBullet(fastBulletPrefab, parent);
		bullet.transform.position = position;
		bullet.transform.localRotation = Quaternion.identity;
		bullet.SetActive(true);
	}

	public void AddToNormalPool(GameObject bullet){
		TryAddToPool(bullet, normalPool);
	}

	public void AddToFastPool(GameObject bullet){
		TryAddToPool(bullet, fastPool);
	}

	GameObject TryGetFromPool(List<GameObject> pool){
		GameObject output = null;
		if(pool.Count > 0){
			output = pool[pool.Count-1];	//should be more efficient than using spot 0
			pool.RemoveAt(pool.Count-1);
		}
		return output;
	}

	GameObject InstantiateBullet(GameObject prefab, Transform parent){
		GameObject bullet = Instantiate(prefab, parent) as GameObject;
		bullet.GetComponent<BulletScript>().bulletPool = this;
		return bullet;
	}

	void TryAddToPool(GameObject obj, List<GameObject> pool){
		if(pool.Count < poolSize){
			pool.Add(obj);
		}else{
			Destroy(obj);
		}
	}

}
