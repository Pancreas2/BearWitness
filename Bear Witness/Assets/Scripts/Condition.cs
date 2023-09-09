using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCondition
{
    public string type = "kindness";
    public int upperBound;
    public int lowerBound;
    public int npcId;

    public bool Evaluate(GameManager gameManager)
    {
        if (type == "kindness")
        {
            NPCData nPCData = gameManager.npcMemory[npcId];
            if (nPCData == null) return false;
            int comparedValue = nPCData.kindness;
            if (upperBound >= comparedValue && lowerBound <= comparedValue)
            {
                return true;
            }
        }
        return false;
    }
}
