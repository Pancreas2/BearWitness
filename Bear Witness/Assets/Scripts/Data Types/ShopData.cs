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
        stock = baseShop.startingStock;
        backgroundDialogue = baseShop.backgroundDialogue;
        name = baseShop.name;
    }
}
