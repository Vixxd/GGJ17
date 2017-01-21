using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    public string playerMoveInput_Name, playerJumpInput_Name;
    public string playerMoveInputController_Name, playerJumpInputController_Name;
    public string playerFire1Input_Name, playerFire1InputController_Name;
    public string playerFire2Input_Name, playerFire2InputController_Name;

    public Rigidbody2D playerRigidBody;
    public GameObject Boat;

    public float playerSpeed = 10f;
    public bool Grounded = false;
    public bool Pushed = false;

    private bool canMove = false;
    private bool isAlive = true;

    private Vector2 initialPos;

    public BaseWeapon playerWeapon1, playerWeapon2;

    public int playerNumber;

    void Start()
    {
        initialPos = transform.position;

        StartCoroutine(getPlayerInput());
	}

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += Instance_OnGameStateChange;
        GameManager.Instance.OnResetCharacter += Instance_OnResetCharacter;
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
                Vector3 boatAngle = Boat.transform.right;

                //Debug.Log(boatAngle);
                if(Grounded)
                {
                    Vector2 boatVelocity = boatAngle * playerSpeed * Input.GetAxis(playerMoveInputController_Name);
                    playerRigidBody.velocity = new Vector2(boatVelocity.x, playerRigidBody.velocity.y);
                }
                else
                {
                    playerRigidBody.velocity = new Vector2(Input.GetAxis(playerMoveInputController_Name) * playerSpeed, playerRigidBody.velocity.y);
                }

                if(Pushed)
                {
                    playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, playerRigidBody.velocity.y);
                }

                if (Input.GetAxis(playerJumpInputController_Name) > 0 && Grounded)
                {
                    Grounded = false;
                    Pushed = false;

                    //playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, Input.GetAxis(playerJumpInput_Name) * 5);
                    playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, Input.GetAxis(playerJumpInputController_Name) * 5);
                }

                if(Input.GetAxis(playerFire1InputController_Name) > 0)
                {
                    playerWeapon1.FireWeapon();
                }

                if (Input.GetAxis(playerFire2InputController_Name) > 0)
                {
                    playerWeapon2.FireWeapon();
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
                playerRigidBody.velocity = Vector2.zero;
                playerRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                break;
            case GameEnums.GameState.Game:
                canMove = true;
                isAlive = true;

                playerRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                break;
            case GameEnums.GameState.GameOver:
                canMove = false;
                break;
            default:
                break;
        }
    }

    private void Instance_OnResetCharacter()
    {
        transform.position = initialPos;
        playerRigidBody.velocity = Vector2.zero;
    }


}
