using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public string entersFromRoom;
    public Vector2 forceOnSpawn = Vector2.zero;
    private void Start()
    {
        if (entersFromRoom != FindFirstObjectByType<GameManager>().previousLevel)
        {
            Destroy(gameObject);
        } else
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        player.transform.position = transform.position;
        player.GetComponent<Rigidbody2D>().AddForce(forceOnSpawn);
    }
}
