using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class LoadingZone : MonoBehaviour
{
    public Vector2 direction;

    public string room;

    public UnityEvent OnRoomWasLoaded;

    void Start()
    {
        if (room == GameManager.instance.previousLevel)
        {
            OnRoomWasLoaded.Invoke();
        }
    }
}
