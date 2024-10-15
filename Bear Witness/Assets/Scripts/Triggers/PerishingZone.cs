using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerishingZone : MonoBehaviour
{
    private PlayerController player;
    private PlayerMovement playerM;
    private GameManager gameManager;

    private float freezeTime = 0f;
    private bool playerFrozen;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        playerM = FindObjectOfType<PlayerMovement>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (playerFrozen && freezeTime <= Time.time)
        {
            playerFrozen = false;
            playerM.frozen = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerM.frozen = true;
            player.SetGravityFraction(0f);
            player.m_Rigidbody2D.velocity = Vector3.zero;
            StartCoroutine(RespawnPlayer());
        }
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        playerM.PlayAnimation("stun");
        player.SetGravityFraction(1f);
        playerFrozen = true;
        freezeTime = Time.time + 1.25f;
        gameManager.RespawnPlayer();
        player.Damage(1, 0, false);
    }
}
