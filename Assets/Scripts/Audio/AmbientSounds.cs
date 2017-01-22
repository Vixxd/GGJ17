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
	

    private void Instance_OnGameStateChange(GameEnums.GameState gameState)
    {
        if (gameState == GameEnums.GameState.Game)
        {
            PlayStartSound();
            StartCoroutine(ReturnToBackGround());
        }
    }

    private void PlayStartSound()
    {
        AudioSource.clip = StartSound;
        AudioSource.loop = false;
        AudioSource.Play();
    }

    private void PlayBackground()
    {
        AudioSource.clip = BackGroundSound;
        AudioSource.loop = true;
        AudioSource.Play();
    }

    private IEnumerator ReturnToBackGround()
    {
        while (AudioSource.isPlaying)
        {
            yield return null;
        }

        PlayBackground();
    }
}
