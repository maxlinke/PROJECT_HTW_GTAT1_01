using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour {

	[SerializeField] GameObject normalModel;
	[SerializeField] GameObject shinyModel;

	MeshRenderer normalMR;
	MeshRenderer shinyMR;
	MaterialPropertyBlock shinyPropBlock;

	Color shineColor;
	float shineIntensity;
	bool isBlinking;
	float nextBlink;

	const float blinkInterval = 0.1f;
	const float shineFalloff = 4f;

	void Start(){
		normalMR = normalModel.GetComponent<MeshRenderer>();
		shinyMR = shinyModel.GetComponent<MeshRenderer>();
		shinyPropBlock = new MaterialPropertyBlock();
		shinyMR.GetPropertyBlock(shinyPropBlock);
		Reset();
	}

	void Update(){
		shinyPropBlock.SetColor("_Color", shineColor);
		shinyPropBlock.SetFloat("_Intensity", shineIntensity);
		shinyMR.SetPropertyBlock(shinyPropBlock);
		ShineManager();
		BlinkManager();
	}

	public void Hide(){
		SetMeshRenderersActive(false);
	}

	public void Unhide(){
		SetMeshRenderersActive(true);
	}

	void SetMeshRenderersActive(bool value){
		normalMR.enabled = value;
		shinyMR.enabled = value;
	}

	public void Reset(){
		Unhide();
		shineColor = new Color(0,0,0,1);
		shineIntensity = 0f;
		isBlinking = false;
	}

	public void SetLocalEulerAngles(Vector3 localEulerAngles){
		transform.localEulerAngles = localEulerAngles;
	}

	public void SetBlinking(bool blink){
		if(blink){
			normalMR.enabled = false;
			nextBlink = Time.time + blinkInterval;
		}else{
			normalMR.enabled = true;
		}
		isBlinking = blink;
	}

	public void Shine(Color color){
		shineColor = color;
		shineIntensity = 1f;
	}

	void ShineManager(){
		if(shineIntensity > 0f){
			shineIntensity -= Time.deltaTime * shineFalloff;
			if(shineIntensity < 0f) shineIntensity = 0f;
		}
	}

	void BlinkManager(){
		if(isBlinking){
			if(Time.time >= nextBlink){
				normalMR.enabled = !normalMR.enabled;
				nextBlink = Time.time + blinkInterval;
			}
		}
	}

}
