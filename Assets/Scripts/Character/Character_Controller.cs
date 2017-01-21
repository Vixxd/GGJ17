﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    public string playerMoveInput_Name, playerJumpInput_Name;
    public string playerMoveInputController_Name, playerJumpInputController_Name;

    public Rigidbody2D playerRigidBody;

    public float playerSpeed = 5.0f;
    public bool Grounded = false;

    private bool canMove = false;
    private bool isAlive = true;

    private Vector2 initialPos;

    void Start()
    {
        initialPos = transform.position;

        StartCoroutine(getPlayerInput());
	}

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += Instance_OnGameStateChange;
    }

    //void OnDisable()
    //{
    //    GameManager.Instance.OnToggleInput -= Instance_OnToggleInput;
    //}

    private IEnumerator getPlayerInput()
    {
        while(true)
        {
            if (canMove)
            {
                //playerRigidBody.velocity = new Vector2(Input.GetAxis(playerMoveInput_Name) * speed, playerRigidBody.velocity.y);
                playerRigidBody.velocity = new Vector2(Input.GetAxis(playerMoveInputController_Name) * playerSpeed, playerRigidBody.velocity.y);

                if (Input.GetAxis(playerJumpInputController_Name) > 0 && Grounded)
                {
                    Grounded = false;

                    //playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, Input.GetAxis(playerJumpInput_Name) * 5);
                    playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, Input.GetAxis(playerJumpInputController_Name) * 5);
                }
            }

            yield return null;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Sea")
        {
            isAlive = false;

            GameManager.Instance.TriggerOnGameStateChange(GameEnums.GameState.GameOver);
        }
    }

    private void Instance_OnGameStateChange(GameEnums.GameState gameState)
    {
        switch(gameState)
        {
            case GameEnums.GameState.Start:
                canMove = false;
                transform.position = initialPos;
                break;
            case GameEnums.GameState.Game:
                canMove = true;
                isAlive = true;
                break;
            case GameEnums.GameState.GameOver:
                canMove = false;
                break;
            default:
                break;
        }
    }
}