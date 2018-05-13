using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInput{

	public static PlayerInput Get(InputType inputType){
		switch(inputType){
		case InputType.KEYBOARD:
			return new PlayerKeyboardInput();
		case InputType.GAMEPAD1:
			return new PlayerGamepadInput(1);
		case InputType.GAMEPAD2:
			return new PlayerGamepadInput(2);
		default:
			throw new UnityException("unknown input type");
		}
	}

	public abstract Vector2 GetMoveInput();

	public abstract bool GetLeftDodgeInputDown();

	public abstract bool GetRightDodgeInputDown();

	public abstract bool GetFireInput();

	public abstract bool GetSpecialInputDown();



}
