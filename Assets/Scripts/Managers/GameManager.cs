using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameManager() { }

	void Start()
    {
		
	}

    // Event Handler
    public delegate void OnToggleInputEvent(bool canMove);
    public event OnToggleInputEvent OnToggleInput;
}
