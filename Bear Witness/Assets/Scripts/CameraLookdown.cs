using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLookdown : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;
    private PlayerMovement player;
    private CinemachineFramingTransposer trans;

    private float waitTime = 0.5f;
    private float startWaitTime = -1f;

    private float minOffset = -2.5f;
    private float maxOffset = 1.5f;
    private float currentOffset = 0f;

    private float step = 0.01f;


    void Start()
    {
        player = FindObjectOfType<PlayerMovement>(); 
        trans = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        if (player)
        {
            if (player.lookingDown)
            {
                if (startWaitTime < 0f)
                {
                    startWaitTime = Time.time;
                }
                else if (startWaitTime + waitTime < Time.time)
                {
                    currentOffset = trans.m_TrackedObjectOffset.y;
                    trans.m_TrackedObjectOffset.y = Mathf.Max(minOffset, currentOffset - step);
                }
            }
            else
            {
                currentOffset = trans.m_TrackedObjectOffset.y;
                trans.m_TrackedObjectOffset.y = Mathf.Min(maxOffset, currentOffset + step);
                startWaitTime = -1f;
            }
        }
    }
}
