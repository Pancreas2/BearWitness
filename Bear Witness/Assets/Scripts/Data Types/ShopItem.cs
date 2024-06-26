using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string name = "ITEM NAME";
    public Item item;
    public int price;
    public bool finiteStock;
    public int stock;
}
