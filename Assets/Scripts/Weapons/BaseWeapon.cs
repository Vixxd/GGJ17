using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public Character_Controller Character_Controller;

    public float FireTimePeriod;
    public float FireRechargeDelayTime;

    public AudioSource AudioSource;
    public AudioClip Attack_Sound;

    protected bool canFire = true;

    public delegate void OnFireEvent();
    public event OnFireEvent OnFire;

    public void TriggerOnFire()
    {
        if (OnFire != null)
        {
            Character_Controller.PlayerAnimator.SetTrigger("Attacking");
            OnFire();
        }
    }

    public virtual void FireWeapon() { }
}
