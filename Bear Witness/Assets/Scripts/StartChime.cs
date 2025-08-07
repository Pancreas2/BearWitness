using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartChime : MonoBehaviour
{
    [SerializeField] AudioSource sound;
    void Start()
    {
        if (GameManager.instance.previousLevel != "Start")
        {
            sound.mute = true;
        }
    }
}
