using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public bool spawnOnLoad = true;
    public LoadingZone loadingZone;

    private Vector3 entranceDirection;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (spawnOnLoad)
        {
            entranceDirection = -loadingZone.direction;

            if (entranceDirection.x != 0)
            {
                Vector3 newPosition = new(transform.position.x, 0.4f + loadingZone.transform.position.y - 0.5f * loadingZone.transform.localScale.y, 0f);
                transform.position = newPosition;
            }

        }

        if (loadingZone.room == gameManager.previousLevel && spawnOnLoad)
        {
            AssignAsSpawn();
            Spawn();
        }
    }

    public void Spawn()
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        if (entranceDirection.x != 0)
        {
            player.transform.position = transform.position - 0.5f * entranceDirection;
            player.GetComponent<PlayerMovement>().WalkToPoint(transform.position.x, entranceDirection.x > 0);
        } else
        {
            player.transform.position = transform.position;
            if (entranceDirection.y > 0)
            {
                Vector2 force = new(0f, 350f);
                player.GetComponent<Rigidbody2D>().AddForce(force);
            }
        }
    }

    public void AssignAsSpawn()
    {
        gameManager.ChangeSpawnPoint(this);
    }
}
