using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : MonoBehaviour
{
    public CollectableItem item;
    [SerializeField] private bool renewsOnSave = false;
    private GameManager gameManager;
    [SerializeField] private string id;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (renewsOnSave)
        {
            if (gameManager.foundItems.Contains(id))
            {
                Destroy(gameObject);
            }
        } else
        {
            if (gameManager.permanentFoundItems.Contains(id))
            {
                Destroy(gameObject);
            }
        }
    }
    public void CollectItem()
    {
        gameManager.PickupItem(item);
        if (renewsOnSave)
        {
            gameManager.foundItems.Add(id);
        } else
        {
            gameManager.permanentFoundItems.Add(id);
        }
        Destroy(gameObject);
    }
}
