using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private string currentBGMusic;

    private LevelLoader levelLoader;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            // for some reason this is necessary ???
            gameObject.SetActive(false);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Start()
    {
        OnLevelWasLoaded(0);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (instance != this) return;
        levelLoader = FindObjectOfType<LevelLoader>();
        if (currentBGMusic != levelLoader.levelMusic)
        {
            Stop(currentBGMusic);
            currentBGMusic = levelLoader.levelMusic;
            Play(currentBGMusic);
        }
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null) return;
        sound.source.Play();
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null) return;
        sound.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound sound in sounds) {
            sound.source.Stop();
        }
    }
}
