using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ChargeWeapon : BaseWeapon
{
    public Character_Controller Character_Controller;

    public float PlayerSpeedModifier = 3f;

    private bool canFire = true;

    public override void OnFire()
    {
        if(canFire)
        {
            StartCoroutine(changePlayerSpeed());
        }
    }

    private IEnumerator changePlayerSpeed()
    {
        canFire = false;
        float originalSpeed = Character_Controller.playerSpeed;

        Character_Controller.playerSpeed = originalSpeed * PlayerSpeedModifier;
        yield return new WaitForSeconds(2f);
        Character_Controller.playerSpeed = originalSpeed;
        canFire = true;
    }
}
