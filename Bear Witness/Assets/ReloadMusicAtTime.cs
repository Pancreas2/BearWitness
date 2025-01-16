using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadMusicAtTime : MonoBehaviour
{

    [SerializeField] private AppearAtTime.Events time;
    private float eventTime;
    private LevelLoader.LevelArea area;
    private bool eventInPast = false;

    void Start()
    {
        eventTime = AppearAtTime.EventMatch[time];
        area = FindObjectOfType<LevelLoader>().area;
        eventInPast = eventTime < GameManager.instance.gameTime;
    }

    void Update()
    {
        if (!eventInPast)
        {
            float currentTime = GameManager.instance.gameTime;
            if (eventTime < currentTime)
            {
                AudioManager.instance.ReloadMusic(AudioManager.instance.AreaMusicMatch(area));
                eventInPast = true;
            }
        }
    }
}
