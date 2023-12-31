using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public string entersFromRoom;
    public Vector2 forceOnSpawn = Vector2.zero;
    public bool spawnOnLoad = true;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Debug.Log(gameManager.previousLevel);
        if (entersFromRoom == gameManager.previousLevel && spawnOnLoad)
        {
            AssignAsSpawn();
            Spawn();
        } else if (spawnOnLoad)
        {
            Debug.Log("Destroying spawn point for entry from " + entersFromRoom);
            Destroy(gameObject);
        }
    }

    public void Spawn()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        player.transform.position = transform.position;
        player.GetComponent<Rigidbody2D>().AddForce(forceOnSpawn);
    }

    public void AssignAsSpawn()
    {
        gameManager.ChangeSpawnPoint(this);
    }
}
