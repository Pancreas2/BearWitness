using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using static Gate;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;

    private List<Sound> playingSounds = new();

    public static AudioManager instance;

    private string currentBGMusic;
    private string prevBGMusic;
    private float prevBGMusicTime;

    private LevelLoader levelLoader;

    public string AreaMusicMatch(LevelLoader.LevelArea area)
    {
        string music = "Intemperance";

        if (GameManager.instance.panicMode)
        {
            return "Fragmentation";
        }

        switch (area) {
            case LevelLoader.LevelArea.Arktis:
                music = "Arktis";
                break;

            case LevelLoader.LevelArea.Airship:
                music = "Airship";
                break;

            case LevelLoader.LevelArea.Shores:
                if (GameManager.instance.gameTime < AppearAtTime.EventMatch[AppearAtTime.Events.ShoreStorm])
                {
                    music = "Shores";
                }
                else
                {
                    music = "Stormy_Shores";
                }
                break;

            case LevelLoader.LevelArea.Hollow:
                music = "Overgrown_Hollow";
                break;

            case LevelLoader.LevelArea.Lighthouse:
                music = "Lighthouse";
                break;

            case LevelLoader.LevelArea.ShoresVillage:
                music = "Village";
                break;

            case LevelLoader.LevelArea.Sigilroom:
                Gate.Gates sigil = FindObjectOfType<DoorOpener>().GetGateName();
                if (GameManager.instance.doorStates[Gate.GateMatch[sigil]])
                {
                    music = "Sigil_Wake";
                } else
                {
                    music = "Sigil_Sleep";
                }
                break;
        }

        return music;
    }

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
        ReloadMusic();
    }

    public void ReloadMusic(string overrideMusic = "")
    {
        if (instance != this) return;
        levelLoader = FindObjectOfType<LevelLoader>();

        string defaultLevelMusic = AreaMusicMatch(levelLoader.area);
        string levelMusic = defaultLevelMusic;

        // for when one room has different music, but is still in the same area
        if (levelLoader.overrideLevelMusic != "")
        {
            levelMusic = levelLoader.overrideLevelMusic;
        }

        // for overriding music during gameplay (e.g. Shores Storm)
        if (overrideMusic != "")
        {
            levelMusic = overrideMusic;
        }

        if (currentBGMusic != levelMusic)
        {

            if (prevBGMusic == levelMusic && prevBGMusic != "Fragmentation")
            {
                KeepTimePlay(levelMusic, 1f);
            }
            else
            {
                Play(levelMusic);
            }

            prevBGMusicTime = GetSoundTime(currentBGMusic);
            Stop(currentBGMusic);

            prevBGMusic = currentBGMusic;
            currentBGMusic = levelMusic;
        }
        else
        {
            prevBGMusicTime = GetSoundTime(currentBGMusic);
        }
    }

    public void Play(string name, float startTime = 0f, float fadeTime = 1f, float delay = 0f)
    {
        Sound sound = sounds.Find(sound => sound.name == name);
        if (sound == null) return;
        sound.source.volume = 0;
        sound.source.Play();
        sound.source.time = startTime;
        StartCoroutine(FadeIn(sound, fadeTime, delay));
    }

    public void KeepTimePlay(string name, float offset)
    {
        float time = prevBGMusicTime + offset;
        Play(name, time);
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

    public void TempMute(float muteTime = 2f, float muteStrength = 1f)
    {
        foreach (Sound sound in playingSounds)
        {
            float originalVolume = sound.source.volume;
            sound.source.volume *= muteStrength;
            StartCoroutine(RestoreVolumeAfter(sound, muteTime, originalVolume));
        }
    }

    private IEnumerator RestoreVolumeAfter(Sound sound, float delay, float volume)
    {
        yield return new WaitForSeconds(delay);
        sound.source.volume = volume;
    }

    private IEnumerator FadeOut(Sound sound, float time)
    {
        float initialVolume = sound.volume;

        if (time > 0f)
            for (int i = 9; i >= 0; i--)
            {
                sound.source.volume = i * initialVolume / 10;
                yield return new WaitForSeconds(time / 10f);
            }
        else yield return new WaitForEndOfFrame();

        playingSounds.Remove(sound);
        sound.source.Stop();
    }

    private IEnumerator FadeIn(Sound sound, float time, float delay)
    {
        yield return new WaitForSeconds(delay);

        float initialVolume = sound.volume;

        for (int i = 1; i <= 10; i++)
        {
            sound.source.volume = i * initialVolume / 10;
            yield return new WaitForSeconds(time / 10);
        }

        playingSounds.Add(sound);
    }

    private float GetSoundTime(string name)
    {
        Sound sound = sounds.Find(sound => sound.name == name);
        if (sound == null) return 0f;
        else return sound.source.time;
    }
}
