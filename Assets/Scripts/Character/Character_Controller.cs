using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    public string playerMoveInput_Name, playerJumpInput_Name;
    public string playerMoveInputController_Name, playerJumpInputController_Name;

    public Rigidbody2D playerRigidBody;

    public float playerSpeed = 5.0f;
    public bool Grounded = false;

    private bool canMove = true;

    void Start()
    {
        StartCoroutine(getPlayerInput());
	}

    void OnEnable()
    {
        GameManager.Instance.OnToggleInput += Instance_OnToggleInput;
    }

    //void OnDisable()
    //{
    //    GameManager.Instance.OnToggleInput -= Instance_OnToggleInput;
    //}

    private IEnumerator getPlayerInput()
    {
        while(canMove)
        {
            //playerRigidBody.velocity = new Vector2(Input.GetAxis(playerMoveInput_Name) * speed, playerRigidBody.velocity.y);
            playerRigidBody.velocity = new Vector2(Input.GetAxis(playerMoveInputController_Name) * playerSpeed, playerRigidBody.velocity.y);

            if (Input.GetAxis(playerJumpInputController_Name) > 0 && Grounded)
            {
                Grounded = false;

                //playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, Input.GetAxis(playerJumpInput_Name) * 5);
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, Input.GetAxis(playerJumpInputController_Name) * 5);
            }


            yield return null;
        }
    }

    private void Instance_OnToggleInput(bool canMove)
    {
        this.canMove = canMove;
    }
}
