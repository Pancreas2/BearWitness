using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Tool,
        Item,
        Wearable
    }
    public ItemType type;
    public new string name;
    public string description;

    public Sprite image;
}
