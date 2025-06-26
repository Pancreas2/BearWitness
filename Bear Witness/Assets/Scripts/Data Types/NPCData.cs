using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCData
{
    public string npc;
    public string displayName;
    public int trust = 0;
    public bool met = false;
    public bool spokenTo = false;
    public string id;
}
