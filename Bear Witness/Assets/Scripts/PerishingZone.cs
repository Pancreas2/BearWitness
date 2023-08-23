using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerishingZone : MonoBehaviour
{
    private PlayerController player;
    private SpawnPlayer respawn;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        respawn = FindObjectOfType<SpawnPlayer>();
    }

    public void ChangeRespawn(SpawnPlayer newRespawn)
    {
        respawn = newRespawn;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (!respawn) respawn = FindObjectOfType<SpawnPlayer>();
            respawn.Spawn();
        }
    }
}
