using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePickup : MonoBehaviour
{
    [SerializeField] private bool oncePerFile = false;
    [SerializeField] private string id;
    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (!oncePerFile)
        {
            if (gameManager.foundItems.Contains(id))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (gameManager.permanentFoundItems.Contains(id))
            {
                Destroy(gameObject);
            }
        }

    }

    public void Collect()
    {
        if (!oncePerFile)
        {
            gameManager.foundItems.Add(id);
        }
        else
        {
            gameManager.permanentFoundItems.Add(id);
        }
        Destroy(gameObject);
    }
}
