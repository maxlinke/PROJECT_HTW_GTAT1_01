﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGamepadInput : PlayerInput {

	string axis_move_x;
	string axis_move_y;

	KeyCode button_dodge_left;
	KeyCode button_dodge_right;
	KeyCode button_fire;
	KeyCode button_special;

	public PlayerGamepadInput(int playernumber) : base(){
		ReloadControls(playernumber);
	}

	void ReloadControls(int playernumber){
		axis_move_x = ParseAxisFromPlayerPrefs("axis_move_x", playernumber);
		axis_move_y = ParseAxisFromPlayerPrefs("axis_move_y", playernumber);
		button_dodge_left = ParseButtonFromPlayerPrefs("button_dodge_left", playernumber);
		button_dodge_right = ParseButtonFromPlayerPrefs("button_dodge_right", playernumber);
		button_fire = ParseButtonFromPlayerPrefs("button_fire", playernumber);
		button_special = ParseButtonFromPlayerPrefs("button_special", playernumber);
	}

	string ParseAxisFromPlayerPrefs(string key, int playernumber){
		string axisGeneric = PlayerPrefManager.GetString(key);
		char numberChar = playernumber.ToString().ToCharArray()[0];
		return axisGeneric.Replace('#', numberChar);
	}

	KeyCode ParseButtonFromPlayerPrefs(string key, int playernumber){
		string buttonGeneric = PlayerPrefManager.GetString(key);
		char numberChar = playernumber.ToString().ToCharArray()[0];
		string keycode = buttonGeneric.Replace('#', numberChar);
		return (KeyCode)System.Enum.Parse(typeof(KeyCode), keycode);
	}

	public override Vector2 GetMoveInput(){
		float xInput = Input.GetAxisRaw(axis_move_x);
		float yInput = Input.GetAxisRaw(axis_move_y) * (-1f);
		return new Vector2(xInput, yInput);
	}

	public override bool GetLeftDodgeInputDown(){
		return Input.GetKeyDown(button_dodge_left);
	}

	public override bool GetRightDodgeInputDown(){
		return Input.GetKeyDown(button_dodge_right);
	}

	public override bool GetFireInput(){
		return Input.GetKey(button_fire);
	}

	public override bool GetSpecialInputDown(){
		return Input.GetKeyDown(button_special);
	}

}
