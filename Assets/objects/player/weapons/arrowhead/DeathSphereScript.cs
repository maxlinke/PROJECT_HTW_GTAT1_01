using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSphereScript : MonoBehaviour {

	[SerializeField] MeshRenderer mr;
	[SerializeField] int damage;
	[SerializeField] float maxRadius;
	[SerializeField] float inflateTime;
	[SerializeField] float waitTime;
	[SerializeField] float deflateTime;

	MaterialPropertyBlock mpb;
	float startTime;
	float inflatedTime;
	float waitedTime;
	float deflatedTime;

	void Start(){
		startTime = Time.time;
		inflatedTime = startTime + inflateTime;
		waitedTime = inflatedTime + waitTime;
		deflatedTime = waitedTime + deflateTime;
		mpb = new MaterialPropertyBlock();
		mr.GetPropertyBlock(mpb);
	}
	
	void Update(){
		if(Time.time < inflatedTime){
			transform.localScale = Vector3.one * maxRadius * (1f - ((inflatedTime - Time.time) / inflateTime));
		}else if(Time.time < waitedTime){
			transform.localScale = Vector3.one * maxRadius;
		}else if(Time.time < deflatedTime){
			transform.localScale = Vector3.one * maxRadius * ((deflatedTime - Time.time) / deflateTime);
		}else{
			Destroy(this.gameObject);
		}
		float averageScale = Vector3.Dot(transform.localScale, Vector3.one) / 3f;
		mpb.SetFloat("_Intensity", averageScale / maxRadius);
		mr.SetPropertyBlock(mpb);
	}

	void OnTriggerEnter(Collider otherCollider){
		IDamageable damageable = otherCollider.gameObject.GetComponent<IDamageable>();
		if(damageable != null){
			damageable.WeaponDamage(damage);
		}
	}
}
