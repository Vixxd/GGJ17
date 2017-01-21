using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_GroundCheck : MonoBehaviour
{
    public Character_Controller character_Controller;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            character_Controller.Grounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            character_Controller.Grounded = false;
        }
    }
}
