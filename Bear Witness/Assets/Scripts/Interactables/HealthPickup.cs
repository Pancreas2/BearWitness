using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private GameManager gameManager;
    private GameUI_Controller guic;
    [SerializeField] private BasicTrigger trigger;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        guic = FindObjectOfType<GameUI_Controller>();
    }

    public void Pickup()
    {
        trigger.enabled = false;
        gameManager.HealPlayer(50f);

        Destroy(gameObject);
    }
}
