using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Door
{
    [SerializeField] private int doorId;
    private GameManager gameManager;
    [SerializeField] private Collider2D collider;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (active)
        {
            if (collider)
            {
                collider.enabled = false;
            }
        }

        active = gameManager.doorStates[doorId];
    }
}
