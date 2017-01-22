using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Splash : MonoBehaviour
{

    public GameObject _ParticleSystem;
    public GameObject _start;
    public GameObject _end;
    public float LerpTime;

    private bool _palying;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnTriggerEnter2D(Collider2D collision)
    {
 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if (!_palying)
            {
                _palying = true;
                StartCoroutine(moveParticleEffect());
            }
            _ParticleSystem.GetComponent<ParticleSystem>().Play();

        }
    }

    private IEnumerator moveParticleEffect()
    {
        float timetotal = LerpTime;
        float timeremaining = timetotal;
        _ParticleSystem.GetComponent<ParticleSystem>().Play();

        while (timeremaining > 0 && _palying)
        {
            timeremaining -= Time.deltaTime;

            float t = (timeremaining / timetotal);
            // Debug.Log("T: " + t + ", Time remaining :" + timeremaining);
            _ParticleSystem.transform.position = Vector3.Lerp(_end.transform.position, _start.transform.position, t);
            yield return null;

        }
        _ParticleSystem.GetComponent<ParticleSystem>().Stop();
        _palying = false;
    }




}
