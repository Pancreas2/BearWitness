using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private PlayerMovement playerM;

    private void Start()
    {
        playerM = FindObjectOfType<PlayerMovement>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                Vector3 snapTo = new(transform.position.x, playerM.transform.position.y);
                playerM.climbing = true;
                playerM.transform.position = snapTo;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerM.climbing = false;
        }
    }
}
