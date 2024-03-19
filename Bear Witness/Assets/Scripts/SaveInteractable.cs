using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInteractable : Interactable
{
    private GameManager gameManager;
    private GameUI_Controller guic;

    private PlayerMovement player;
    private float freezeDelay = 0;
    private bool playerIsFrozen;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        guic = FindObjectOfType<GameUI_Controller>();
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
        audioSource.PlayDelayed(0.2f);
        playerIsFrozen = true;
        player.frozen = true;
        player.PlayAnimation("spin");
        freezeDelay = Time.time + 1f;
        guic.IncreaseHP(gameManager.playerMaxHealth);
        gameManager.SavePlayerData(gameManager.fileNumber);
    }
}
