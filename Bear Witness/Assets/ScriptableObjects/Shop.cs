using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop", menuName = "Shop")]
public class NPCShop : ScriptableObject
{
    public string name;
    public Dialogue backgroundDialogue;
    public List<ShopItem> startingStock;
}
