using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    public string playerMoveInput_Name, playerJumpInput_Name;

    public Rigidbody2D playerRigidBody;

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
        //Vector2 moveVector = new Vector2(0, 0);
        while(canMove)
        {
            if (Input.GetAxis(playerMoveInput_Name) != 0)
            {
                Debug.Log("Left");
                playerRigidBody.velocity = new Vector2(Input.GetAxis(playerMoveInput_Name), playerRigidBody.velocity.y);
            }

            if (Input.GetAxis(playerJumpInput_Name) > 0 && Grounded)
            {
                Debug.Log("Up");
                Grounded = false;
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, Input.GetAxis(playerJumpInput_Name)*5);
            }

            yield return null;
        }
    }

    private void Instance_OnToggleInput(bool canMove)
    {
        this.canMove = canMove;
    }
}
