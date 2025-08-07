using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class ItemSlot : ToolSlot
{
    [SerializeField] private TextMeshProUGUI amountText;

    public override void Reload()
    {
        base.Reload();


        if (invSlot.heldItem && GameManager.instance.ContainsItem(invSlot.heldItem.name))
        {
            int count = GameManager.instance.items.Find(target => target.item == invSlot.heldItem.name).count;
            if (count == 0) amountText.text = "";
            else amountText.text = count.ToString();
        }
        else amountText.text = "";
    }
}
