using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public enum SlotType
    {
        Tool,
        Item,
        Armor
    }

    public SlotType slotType;
    public Item heldItem;
    [SerializeField] private Image imageIcon;
    private InventoryMenu inventoryManager;
    public int index;
    [SerializeField] private Sprite defaultSprite;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryMenu>();

        FindItem();

        if (slotType == SlotType.Tool)
            inventoryManager.toolSlots[index] = this;
        else
            inventoryManager.itemSlots[index] = this;
        
    }

    public void FindItem()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (slotType == SlotType.Tool)
        {
            heldItem = gameManager.tools[index];
        }
        else if (slotType == SlotType.Item)
        {
            heldItem = gameManager.items[index];
        }
    }

    public void ReloadImage()
    {
        if (heldItem == null || heldItem.image == null)
        {
            imageIcon.sprite = defaultSprite;
        } else
        {
            imageIcon.sprite = heldItem.image;
        }
    }

    public void RemoveItem()
    {
        heldItem = null;
    }

    public void SetItem(Item newItem)
    {
        heldItem = newItem;
    }

    public Item SwitchItem(Item newItem)
    {
        Item oldItem = heldItem;
        heldItem = newItem;
        return oldItem;
    }

    public void SwapWithActive()
    {
        // get the previously selected inventory slot
        InventorySlot activeSlot = inventoryManager.lastSelectedSlot;
        if (!activeSlot) return;

        // if the active slot and new slot are in different inventories, return
        if (activeSlot.slotType != slotType) {
            inventoryManager.lastSelectedSlot = this;
            return;
        }

        // switch which item is held in each slot
        Item currentItem = SwitchItem(activeSlot.heldItem);
        activeSlot.SetItem(currentItem);

        // switch the positions of the items in the game data
        inventoryManager.SwapInventoryPosition(activeSlot.index, index, slotType == SlotType.Tool);

        // show the change
        activeSlot.ReloadImage();
        ReloadImage();

        Debug.Log("switching " + index + " and " + activeSlot.index);
        inventoryManager.lastSelectedSlot = this;
    }
}
