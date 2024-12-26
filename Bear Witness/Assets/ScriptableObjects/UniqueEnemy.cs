using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        int index = (int)name;
        Debug.Log(index);
        if (gameManager.uniqueEnemies[index])
        {
            Destroy(gameObject);
        }
    }

    public enum UniqueEnemyName
    {
        ArmouredMollusk,
        CaptainCarlotta,
        RockCrab
    }

    public UniqueEnemyName name;

    public void UniqueEnemySlain()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.GetComponent<FreezeFrame>().Slow(1f, 0.05f);
        int index = (int)name;
        gameManager.uniqueEnemies[index] = true;
    }

    public bool IsAlive()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        int index = (int)name;
        return !gameManager.uniqueEnemies[index];
    }
}
