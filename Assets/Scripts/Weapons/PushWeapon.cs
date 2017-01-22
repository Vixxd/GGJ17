using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PushWeapon : BaseWeapon
{
    public float pushedForTime = 1f;

    void Start()
    {
        AudioSource = gameObject.GetComponent<AudioSource>();

        FireTimePeriod = 0.1f;
        FireRechargeDelayTime = 2f;
    }

    public override void FireWeapon()
    {
        if(canFire)
        {
            Debug.Log("FIRE");
            TriggerOnFire();
            StartCoroutine(fireWeapon());
        }
    }

    private IEnumerator fireWeapon()
    {
        canFire = false;

        AudioSource.PlayOneShot(Attack_Sound);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(new Vector3(4, 4, 4), FireTimePeriod));
        seq.Append(transform.DOScale(new Vector3(1, 1, 1), FireTimePeriod/10));
        seq.Play();

        yield return new WaitForSeconds(FireTimePeriod + FireRechargeDelayTime);
        canFire = true;
    }

    private IEnumerator playerPushedDelay(Character_Controller character_Controller)
    {
        character_Controller.Pushed = true;
        yield return new WaitForSeconds(pushedForTime);
        character_Controller.Pushed = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Vector3 otherPosition = col.transform.position;
            Vector3 diffPosition = transform.position - otherPosition;

            col.gameObject.GetComponent<Rigidbody2D>().velocity = (-diffPosition.normalized);

            Vector2 playerVelocity = Character_Controller.playerRigidBody.velocity.normalized;

            if (col.transform.position.x > transform.position.x)
            {
                col.gameObject.GetComponent<Rigidbody2D>().velocity = (new Vector2(2, 3));
            }
            else
            {
                col.gameObject.GetComponent<Rigidbody2D>().velocity = (new Vector2(-2, 3));
            }

            StartCoroutine(playerPushedDelay(col.gameObject.GetComponent<Character_Controller>()));
        }
    }
}
