using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThroughPlatform : MonoBehaviour
{
    bool playerOnPlatform = false;
    Collider2D collider;
    PlayerMovement player;
    bool playerWasClimbing = false;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetOnPlatform(collision.collider, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        SetOnPlatform(collision.collider, false);
    }

    private void SetOnPlatform(Collider2D otherCollider, bool value)
    {
        if (player != null)
        {
            playerOnPlatform = value;
            player.onPassThroughPlatform = value;
        }
    }

    private void Update()
    {
        if (playerOnPlatform && Input.GetButtonDown("Jump") && !player.frozen)
        {
            StartCoroutine(EnableCollider());
        }

        if (player.climbing)
        {
            collider.enabled = false;
            playerWasClimbing = true;
        } else if (playerWasClimbing)
        {
            playerWasClimbing = false;
            collider.enabled = true;
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForFixedUpdate();
        collider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        collider.enabled = true;
    }
}
