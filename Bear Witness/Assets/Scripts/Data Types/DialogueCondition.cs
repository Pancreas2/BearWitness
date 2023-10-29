using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCondition
{
    public enum ConditionType
    {
        TrustLevel,
        PreviousLinePlayed, 
        Money
    }
    public ConditionType type;
    [Header("Trust Level")]
    public int upperBound;
    public int lowerBound;
    public int npcId;
    [Header("PreviousLinePlayed")]
    public string lineId;
    [Header("Money")]
    public int threshold;
    public bool greaterThan;
    

    public bool Evaluate(GameManager gameManager)
    {
        if (type == ConditionType.TrustLevel)
        {
            NPCData nPCData = gameManager.npcMemory[npcId];
            if (nPCData == null) return false;
            int comparedValue = nPCData.kindness;
            if (upperBound >= comparedValue && lowerBound <= comparedValue)
            {
                return true;
            }
        } else if (type == ConditionType.PreviousLinePlayed)
        {
            return gameManager.playedLines.Contains(lineId);
        } else if (type == ConditionType.Money)
        {
            return gameManager.money < threshold ^ greaterThan;
        }
        return false;
    }
}
