using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Door
{
    [SerializeField] private Gate.Gates doorName;
    private GameManager gameManager;
    [SerializeField] private GameObject disableOnUnlock;
    bool active = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {

        active = gameManager.doorStates[Gate.GateMatch[doorName]];

        if (active)
        {
            if (disableOnUnlock)
            {
                disableOnUnlock.SetActive(false);
            }
        }
    }
}
