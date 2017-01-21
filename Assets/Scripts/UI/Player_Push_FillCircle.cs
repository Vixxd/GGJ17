using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Push_FillCircle : MonoBehaviour
{
    public Character_Controller Character_Controller;

    private PushWeapon playerPushWeapon;

    private Image pushFillCircle;

    void Start ()
    {
        pushFillCircle = gameObject.GetComponent<Image>();
    }

    void OnEnable()
    {
        playerPushWeapon = (PushWeapon)Character_Controller.playerWeapon1;
        playerPushWeapon.OnFire += PlayerPushWeapon_OnFire;
    }

    private void PlayerPushWeapon_OnFire()
    {
        StartCoroutine(decayPushWeaponFillCircle());
    }

    private IEnumerator decayPushWeaponFillCircle()
    {
        while (pushFillCircle.fillAmount > 0)
        {
            pushFillCircle.fillAmount -= (1 / playerPushWeapon.FireTimePeriod) * Time.deltaTime;
            yield return null;
        }

        while (pushFillCircle.fillAmount < 1)
        {
            pushFillCircle.fillAmount += (1 / playerPushWeapon.FireRechargeDelayTime) * Time.deltaTime;
            yield return null;
        }
    }
}
