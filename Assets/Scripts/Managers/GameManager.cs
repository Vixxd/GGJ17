using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameManager() { }

    public GameEnums.GameState CurrentGameState { get; set; }
    public List<GameObject> playerGameObject_List;

	void Start()
    {
        TriggerOnGameStateChange(GameEnums.GameState.Start);
    }

    // Event Handler
    public delegate void OnGameStateChangeEvent(GameEnums.GameState gameState);
    public event OnGameStateChangeEvent OnGameStateChange;

    public void TriggerOnGameStateChange(GameEnums.GameState gameState)
    {
        if(OnGameStateChange != null)
        {
            CurrentGameState = gameState;
            OnGameStateChange(gameState);
        }
    }
}
