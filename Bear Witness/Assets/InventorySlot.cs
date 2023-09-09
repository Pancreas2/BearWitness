using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public CollectableItem heldItem = null;

    public void RemoveItem()
    {
        heldItem = null;
    }

    public void AddItem(CollectableItem newItem)
    {
        heldItem = newItem;
    }

    public CollectableItem SwitchItem(CollectableItem newItem)
    {
        CollectableItem oldItem = heldItem;
        heldItem = newItem;
        return oldItem;
    }
}
