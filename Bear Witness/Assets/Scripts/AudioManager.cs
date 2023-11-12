using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private List<Sound> playingSounds = new();

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

    public void Play(string name, float startTime = 0f, float fadeTime = 1f)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null) return;
        sound.source.volume = 0;
        sound.source.Play();
        StartCoroutine(FadeIn(sound, fadeTime));
        sound.source.time = startTime;
        playingSounds.Add(sound);
    }

    public void Stop(string name, float fadeTime = 1f)
    {
        Sound sound = playingSounds.Find(sound => sound.name == name);
        if (sound == null) return;
        StartCoroutine(FadeOut(sound, fadeTime));
    }

    public void StopAll(float fadeTime = 1f)
    {
        foreach (Sound sound in playingSounds) {
            StartCoroutine(FadeOut(sound, fadeTime));
        }
    }

    private IEnumerator FadeOut(Sound sound, float time)
    {
        float initialVolume = sound.volume;

        for (int i = 9; i >= 0; i--)
        {
            sound.source.volume = i * initialVolume / 10;
            yield return new WaitForSeconds(time / 10f);
        }

        playingSounds.Remove(sound);
        sound.source.Stop();
    }

    private IEnumerator FadeIn(Sound sound, float time)
    {
        float initialVolume = sound.volume;

        for (int i = 1; i <= 10; i++)
        {
            sound.source.volume = i * initialVolume / 10;
            yield return new WaitForSeconds(time / 10f);
        }
    }
}
