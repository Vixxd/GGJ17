using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject Panel_Start;

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += Instance_OnGameStateChange;
    }

    private IEnumerator waitForInput()
    {
        yield return new WaitForSeconds(1f);
        while(true)
        {
            if (Input.anyKey)
            {
                Panel_Start.SetActive(false);

                GameManager.Instance.TriggerOnGameStateChange(GameEnums.GameState.Game);
                break;
            }
            yield return null;
        }
    }

    private void Instance_OnGameStateChange(GameEnums.GameState gameState)
    {
        if (gameState == GameEnums.GameState.Start)
        {
            Panel_Start.SetActive(true);
            StartCoroutine(waitForInput());
        }
    }
}
