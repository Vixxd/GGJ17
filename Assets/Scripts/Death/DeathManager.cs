using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{

    public float ResetTime = 1.0f;
    public int Lives = 3;
    public bool infiniteLives;

    public int p1Lives;
    public int p2Lives;

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += Instance_OnGameStateChange; ;
    }

    

    // Use this for initialization
    void Start ()
	{
        
        p1Lives = Lives;
	    p2Lives = Lives;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("765875785t78t568765t678565t78");
        if (other.tag == "Player")
        {
            if (other.GetComponent<Character_Controller>().playerNumber == 1)
            {
                p1Lives--;
            }
            else
            {
                p2Lives--;
            }

            if ((p1Lives < 1 || p2Lives < 1)&&!infiniteLives)
            {
                //do game over
                GameManager.Instance.TriggerOnGameStateChange(GameEnums.GameState.GameOver);
            }
            else
            {

                StartCoroutine(ResetLevel());
            }
        }
    }

    private IEnumerator ResetLevel()
    {
        yield return new WaitForSeconds(ResetTime);
        GameManager.Instance.TriggerOnResetCharacter();
    }

    private void Instance_OnGameStateChange(GameEnums.GameState gameState)
    {
        if (gameState == GameEnums.GameState.Game)
        {
            p1Lives = Lives;
            p2Lives = Lives;
        }
    }
}
