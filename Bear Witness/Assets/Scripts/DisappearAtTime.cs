using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearAtTime : MonoBehaviour
{
    [SerializeField] private AppearAtTime.Events start;
    [SerializeField] private AppearAtTime.Events end;
    [SerializeField] private List<GameObject> targets = new();
    bool enabled = true;
    float startTime;
    float endTime;


    private void Update()
    {
        if (GameManager.instance.gameTime > startTime)
        {
            if (GameManager.instance.gameTime > endTime)
            {
                enabled = true;
                foreach (GameObject target in targets)
                {
                    target.SetActive(enabled);
                }
            }
            else if (enabled)
            {
                enabled = false;
                foreach (GameObject target in targets)
                {
                    target.SetActive(enabled);
                }
            }
        }
    }

    private void Start()
    {
        startTime = AppearAtTime.EventMatch[start];
        endTime = AppearAtTime.EventMatch[end];
    }
}