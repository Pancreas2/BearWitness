using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInteractable : Interactable
{
    private GameManager gameManager;

    private PlayerMovement player;
    private float freezeDelay = 0;
    private bool playerIsFrozen;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        if (playerIsFrozen && freezeDelay <= Time.time)
        {
            player.frozen = false;
            playerIsFrozen = false;
        }
    }

    override public void OnInteract()
    {
        playerIsFrozen = true;
        player.frozen = true;
        freezeDelay = Time.time + 0.5f;
        gameManager.SavePlayerData(gameManager.fileNumber);
    }
}
