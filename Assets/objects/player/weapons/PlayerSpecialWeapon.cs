using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSpecialWeapon : MonoBehaviour {

	public abstract void SetWeaponSystem(PlayerWeaponSystem pws);

	public abstract void Fire();

	public abstract void Reload();

	public abstract bool IsEmpty();

	public abstract bool CanFire();

	public abstract float GetTimeToNextFire();

	public abstract bool CanBeReloaded();

	public abstract int GetAmmoCount();

	public abstract void Reset();

}
