using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopData
{
    public string name;
    public Dialogue backgroundDialogue;
    public List<ShopItem> stock = new();

    public void LoadFromBaseShop(NPCShop baseShop)
    {
        foreach (ShopItem item in baseShop.startingStock)
        {
            ShopItem newItem = new();
            newItem.stock = item.stock;
            newItem.price = item.price;
            newItem.name = item.name;
            newItem.finiteStock = item.finiteStock;
            newItem.item = item.item;

            stock.Add(newItem);
        }
        backgroundDialogue = baseShop.backgroundDialogue;
        name = baseShop.name;
    }
}
