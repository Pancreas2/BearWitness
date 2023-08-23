using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : MonoBehaviour
{
    public CollectableItem item;
    public void CollectItem()
    {
        FindObjectOfType<GameManager>().PickupItem(item);
        Destroy(gameObject);
    }
}
