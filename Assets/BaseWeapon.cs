using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public abstract void OnFire();

    protected abstract void OnTriggerEnter2D(Collider2D col);
}
