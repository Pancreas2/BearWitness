using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gate;

public class AppearAtTime : MonoBehaviour
{
    [SerializeField] private Events start;
    [SerializeField] private Events end;
    [SerializeField] private List<GameObject> targets = new();
    bool enabled = false;
    float startTime;
    float endTime;

    public enum Events
    {
        Arrival,
        ShoreStorm,
        End
    }

    public static readonly Dictionary<Events, float> EventMatch = new Dictionary<Events, float>
    {
        { Events.Arrival, 0f },
        { Events.ShoreStorm, 390f },
        { Events.End, 1440f }
    };

    private void Update()
    {
        if (GameManager.instance.gameTime > startTime)
        {
            if (GameManager.instance.gameTime > endTime)
            {
                enabled = false;
                foreach (GameObject target in targets)
                {
                    target.SetActive(enabled);
                }
            } else if (!enabled)
            {
                enabled = true;
                foreach (GameObject target in targets) {
                    target.SetActive(enabled);
                }
            }
        }
    }

    private void Start()
    {
        startTime = EventMatch[start];
        endTime = EventMatch[end];
        foreach (GameObject target in targets)
        {
            target.SetActive(false);
        }
    }
}
