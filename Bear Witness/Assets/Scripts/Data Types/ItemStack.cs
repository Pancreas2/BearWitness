using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public string item;
    public int count;
    public bool empty = true;

    public void Increment(string collectedItem)
    {
        if (empty)
        {
            item = collectedItem;
            count = 1;
            empty = false;
        } else if (item == collectedItem)
        {
            count++;
        } else
        {
            Debug.LogError("Attempted to add item " + collectedItem + "to a stack of " + item);
        }
    }

    public void Decrement()
    {
        if (count > 1)
        {
            count--;
        } else if (count == 1)
        {
            count = 0;
            item = "";
            empty = true;  // if the slot is empty, the item doesn't matter
        } else
        {
            Debug.LogError("Attempted to decrement from a stack of " + count + " " + item);
        }
    }

    public Sprite GetImage()
    {
        if (empty) return Resources.Load<Sprite>("null_tool");
        else return Resources.Load<Item>(item).image;
    }

    public ItemStack()
    {
        Debug.Log("INITIALIZING");
        item = "";
        count = 0;
        empty = true;
    }
}
