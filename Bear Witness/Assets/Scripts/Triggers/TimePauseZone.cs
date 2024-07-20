using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePauseZone : MonoBehaviour
{
    bool active = false;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!active && collision.collider.CompareTag("Player") && !gameManager.pauseGameTime)
        {
            gameManager.pauseGameTime = true;
            active = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (active && collision.collider.CompareTag("Player"))
        {
            gameManager.pauseGameTime = false;
            active = false;
        }
    }
}
