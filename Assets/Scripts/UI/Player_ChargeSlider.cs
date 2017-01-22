using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_ChargeSlider : MonoBehaviour
{
    public Character_Controller Character_Controller;

    private ChargeWeapon playerChargeWeapon;

    private Slider chargeSlider;

	void Awake()
    {
        chargeSlider = gameObject.GetComponent<Slider>();
    }

    void OnEnable()
    {
        playerChargeWeapon = (ChargeWeapon) Character_Controller.playerWeapon2;
        playerChargeWeapon.OnFire += PlayerChargeWeapon_OnFire;

        chargeSlider.value = 1f;
    }

    private void PlayerChargeWeapon_OnFire()
    {
        StopAllCoroutines();
        StartCoroutine(decayChargeWeaponSlider());
    }

    private IEnumerator decayChargeWeaponSlider()
    {
        while (chargeSlider.value > 0)
        {
            chargeSlider.value -= (1 / playerChargeWeapon.FireTimePeriod) * Time.deltaTime;
            yield return null;
        }

        while (chargeSlider.value < 1)
        {
            chargeSlider.value += ( 1/ playerChargeWeapon.FireRechargeDelayTime) * Time.deltaTime;
            yield return null;
        }
    }
}
