using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public CollectableItem heldItem = new();
    [SerializeField] private Image imageIcon;
    private InventoryMenu inventoryManager;
    public int index;
    [SerializeField] private Sprite defaultSprite;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryMenu>();
        inventoryManager.inventorySlots[index] = this;
    }

    public void ReloadImage()
    {
        if (heldItem == null || heldItem.icon == null)
        {
            imageIcon.sprite = defaultSprite;
        } else
        {
            imageIcon.sprite = heldItem.icon;
        }
    }

    public void RemoveItem()
    {
        heldItem = null;
    }

    public void SetItem(CollectableItem newItem)
    {
        heldItem = newItem;
    }

    public CollectableItem SwitchItem(CollectableItem newItem)
    {
        CollectableItem oldItem = heldItem;
        heldItem = newItem;
        return oldItem;
    }

    public void SwapWithActive()
    {
        // get the previously selected inventory slot
        InventorySlot activeSlot = inventoryManager.lastSelectedSlot;
        if (!activeSlot) return;

        // switch which item is held in each slot
        CollectableItem currentItem = SwitchItem(activeSlot.heldItem);
        activeSlot.SetItem(currentItem);

        // switch the positions of the items in the game data
        inventoryManager.SwapInventoryPosition(activeSlot.index, index);

        // show the change
        activeSlot.ReloadImage();
        ReloadImage();

        Debug.Log("switching " + index + " and " + activeSlot.index);
        inventoryManager.lastSelectedSlot = this;
    }
}
