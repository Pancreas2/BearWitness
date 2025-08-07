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
        CircleBadge,
        SquareBadge,
        TriangleBadge,
        Key
    }

    public ItemType type;
    public new string name;
    [TextArea(3, 5)] public string description;

    public Sprite image;
}
