using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour {

    public AudioSource AudioSource;

    public AudioClip BackGroundSound, StartSound;

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += Instance_OnGameStateChange;
    }

    // Use this for initialization
    void Start ()
    {
        AudioSource.clip = BackGroundSound;
        AudioSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Instance_OnGameStateChange(GameEnums.GameState gameState)
    {
        if (gameState == GameEnums.GameState.Game)
        {
            AudioSource.PlayOneShot(StartSound); 
        }
    }
}
