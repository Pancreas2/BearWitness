using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    [SerializeField] private int doorID;
    public void SetDoorState(bool value)
    {
        gameManager.doorStates[doorID] = value;
    }
}
