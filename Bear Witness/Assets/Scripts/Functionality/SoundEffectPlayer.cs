using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    private AudioManager audioManager;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlaySound(string name)
    {
        audioManager.Play(name);
    }

    public void StopSound(string name)
    {
        audioManager.Stop(name);
    }
}
