using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Badge", menuName = "Badge")]
[System.Serializable]
public class Badge : ScriptableObject
{
    public enum BadgeType
    {
        Circle,
        Square,
        Triangle
    }

    public BadgeType badgeType;
    public new string name;
    public string description;
    public Sprite image;
}
