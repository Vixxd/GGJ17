using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{

    public float ResetTime = 1.0f;
    public int Lives = 3;
    public bool infiniteLives;
    public Text VictoryText;


    public int p1Lives;
    public int p2Lives;

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += Instance_OnGameStateChange; ;
    }



    // Use this for initialization
    void Start()
    {
        VictoryText.text = "";
        p1Lives = Lives;
        p2Lives = Lives;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
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

            if ((p1Lives < 1 || p2Lives < 1) && !infiniteLives)
            {
                //do game over
                if (p1Lives > 0)
                {
                    VictoryText.text = "Player 1 Wins!";
                }
                else if (p2Lives > 0)
                {
                    VictoryText.text = "Player 1 Wins!";
                }
                else
                {
                    VictoryText.text = "Draw!";
                }
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
            VictoryText.text = "";
            p1Lives = Lives;
            p2Lives = Lives;
        }
    }
}