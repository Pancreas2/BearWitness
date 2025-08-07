using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolSlot : MonoBehaviour
{
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite equippedSelectedSprite;

    public int equipState = -1;
    [SerializeField] private Sprite unequipped;
    [SerializeField] private Sprite equipX;
    [SerializeField] private Sprite equipY;
    [SerializeField] private Sprite equipB;
    [SerializeField] private Image buttonImage;

    [SerializeField] private Item nullItem;

    public InventorySlot invSlot;


    private void Start()
    {
        invSlot = GetComponent<InventorySlot>();
    }

    public void EquipItem(int slot)
    {
        if (equipState > -1)
        {
            GameManager.instance.currentItems[equipState] = "";
        }

        if (slot == equipState)
        {
            UnequipItem();
            return;
        }

        equipState = slot;
        if (invSlot.heldItem)
            GameManager.instance.currentItems[slot] = invSlot.heldItem.name;
        else GameManager.instance.currentItems[slot] = "";

        Reload();
    }

    public void UnequipItem()
    {
        if (equipState > -1)
        {
            GameManager.instance.currentItems[equipState] = "";
        }

        equipState = -1;

        Reload();
    }

    public virtual void Reload()
    {
        if (invSlot.heldItem && GameManager.instance.currentItems.Contains(invSlot.heldItem.name))
        {
            int slot = GameManager.instance.currentItems.IndexOf(invSlot.heldItem.name);
            switch (slot)
            {
                case 0:
                    buttonImage.sprite = equipX;
                    break;
                case 1:
                    buttonImage.sprite = equipY;
                    break;
                case 2:
                    buttonImage.sprite = equipB;
                    break;
            }

            SpriteState ss = new SpriteState();
            ss.highlightedSprite = equippedSelectedSprite;
            ss.pressedSprite = equippedSelectedSprite;
            ss.selectedSprite = equippedSelectedSprite;
            GetComponent<Button>().spriteState = ss;
        }
        else
        {
            buttonImage.sprite = unequipped;

            SpriteState ss = new SpriteState();
            ss.highlightedSprite = selectedSprite;
            ss.pressedSprite = selectedSprite;
            ss.selectedSprite = selectedSprite;
            GetComponent<Button>().spriteState = ss;
        }
    }
}
