using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    public GameObject Panel_Game;

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += Instance_OnGameStateChange;
    }

    private void Instance_OnGameStateChange(GameEnums.GameState gameState)
    {
        if (gameState == GameEnums.GameState.Game)
        {
            Panel_Game.gameObject.SetActive(true);
        }
        else
        {
            Panel_Game.gameObject.SetActive(false);
        }
    }
}
