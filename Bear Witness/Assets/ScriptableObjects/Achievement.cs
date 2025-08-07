using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement")]
[System.Serializable]
public class Achievement : ScriptableObject
{
    public string name;
    public string description;
    public Sprite image;
    public bool isSecret = false;
    public bool isGold = false;
}
