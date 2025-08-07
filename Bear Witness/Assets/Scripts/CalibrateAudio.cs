using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateAudio : MonoBehaviour
{
    public bool isMusic = false;
    public List<AudioSource> sources;

    private void Start()
    {
        float volFactor = 1f;

        if (isMusic)
        {
            volFactor = PlayerPrefs.GetFloat("music");
        } else
        {
            volFactor = PlayerPrefs.GetFloat("sound");
        }

        foreach (AudioSource source in sources)
        {
            source.volume *= volFactor;
        }
    }
}
