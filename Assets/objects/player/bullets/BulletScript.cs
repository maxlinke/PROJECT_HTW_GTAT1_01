using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	[HideInInspector] public BulletPoolScript bulletPool;
	[SerializeField] Rigidbody rb;
	[SerializeField] int damage;
	[SerializeField] float speed;
	[SerializeField] float lifeTime;
	[SerializeField] bool isFastBullet;

	float killTime;

	void OnEnable(){
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		killTime = Time.time + lifeTime;
	}

	void Update () {
		if(Time.time >= killTime){
			ReturnToPool();
		}else{
			transform.localPosition += Vector3.forward * speed * Time.deltaTime;
		}
	}

	void OnCollisionEnter(Collision collision){
		IDamageable damageable = collision.collider.gameObject.GetComponent<IDamageable>();
		if(damageable != null) damageable.WeaponDamage(damage);
		ReturnToPool();
	}

	void ReturnToPool(){
		if(isFastBullet) bulletPool.AddToFastPool(this.gameObject);
		else bulletPool.AddToNormalPool(this.gameObject);
		this.gameObject.SetActive(false);
	}

}
