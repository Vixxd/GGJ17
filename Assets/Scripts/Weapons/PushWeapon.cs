using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PushWeapon : BaseWeapon
{
    void Start()
    {
        FireTimePeriod = 0.1f;
        FireRechargeDelayTime = 2f;
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

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(new Vector3(4, 4, 4), FireTimePeriod));
        seq.Append(transform.DOScale(new Vector3(1, 1, 1), FireRechargeDelayTime));
        seq.Play();

        yield return new WaitForSeconds(FireRechargeDelayTime);
        canFire = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Vector3 otherPosition = col.transform.position;
            Vector3 diffPosition = transform.position - otherPosition;

            col.gameObject.GetComponent<Rigidbody2D>().AddForce(-diffPosition * 300);
        }
    }
}
