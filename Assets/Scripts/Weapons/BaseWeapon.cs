using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public delegate void OnFireEvent();
    public event OnFireEvent OnFire;

    public void TriggerOnFire()
    {
        if (OnFire != null)
        {
            OnFire();
        }
    }

    public virtual void FireWeapon() { }
}
