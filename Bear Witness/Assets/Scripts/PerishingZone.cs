using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerishingZone : MonoBehaviour
{
    private PlayerController player;
    private GameManager gameManager;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            gameManager.RespawnPlayer();
            player.Damage(1, 0, false);
        }
    }
}
