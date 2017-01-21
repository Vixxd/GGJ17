using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PushWeapon : BaseWeapon
{
	public override void FireWeapon()
    {
        TriggerOnFire();
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(new Vector3(4, 4, 4), 0.1f));
        seq.Append(transform.DOScale(new Vector3(1, 1, 1), 0.01f));

        seq.Play();
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
