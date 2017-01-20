using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour {

	//Here's the video I followed to do this and the explanation for two cameras
	//https://www.youtube.com/watch?v=9bhkH7mtFNE
	//And here's why the code doesnt work straight out of the video
	//http://answers.unity3d.com/questions/909215/unity-5-gameobjectrenderermaterial.html

	public float parallaxSpeed = 0;
	
	// Update is called once per frame
	void Update () {

		//As time elapses, shift the UV co-ordinates of this texture on this object
		//This will make the texture repeat by default
		gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2 (Time.time * parallaxSpeed, 0f);
	}
}
