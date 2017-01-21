using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public GameObject Panel_GameOver;

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += Instance_OnGameStateChange;
    }

    private IEnumerator waitForInput()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (Input.anyKey)
            {
                Panel_GameOver.gameObject.SetActive(false);

                GameManager.Instance.TriggerOnGameStateChange(GameEnums.GameState.Start);
                break;
            }
            yield return null;
        }
    }

    private void Instance_OnGameStateChange(GameEnums.GameState gameState)
    {
        if(gameState == GameEnums.GameState.GameOver)
        {
            Panel_GameOver.gameObject.SetActive(true);
            StartCoroutine(waitForInput());
        }
    }
}
