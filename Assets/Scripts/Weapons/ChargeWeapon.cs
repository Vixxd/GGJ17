using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ChargeWeapon : BaseWeapon
{
    public float PlayerSpeedModifier = 3f;

    void Start()
    {
        FireTimePeriod = 2f;
        FireRechargeDelayTime = 3f;
    }

    public override void FireWeapon()
    {
        if(canFire)
        {
            TriggerOnFire();
            StartCoroutine(fireWeapon());
        }
    }

    private IEnumerator fireWeapon()
    {
        canFire = false;
        float originalSpeed = Character_Controller.playerSpeed;

        Character_Controller.playerSpeed = originalSpeed * PlayerSpeedModifier;
        yield return new WaitForSeconds(FireTimePeriod);
        Character_Controller.playerSpeed = originalSpeed;
        yield return new WaitForSeconds(FireRechargeDelayTime);
        canFire = true;
    }
}
