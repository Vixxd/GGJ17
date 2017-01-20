using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour {

	public float speed = 0;
	
	// Update is called once per frame
	void Update () {

		//As time elapses, shift the UV co-ordinates of this texture on this object
		//This will make the texture repeat by default
		gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2 (Time.time * speed, 0f);

		
	}
}
