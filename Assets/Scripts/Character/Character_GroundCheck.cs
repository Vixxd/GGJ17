﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_GroundCheck : MonoBehaviour
{
    public Character_Controller character_Controller;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            character_Controller.Grounded = true;
        }
    }
}