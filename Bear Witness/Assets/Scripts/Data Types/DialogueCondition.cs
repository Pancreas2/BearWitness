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
        Money,
        HasItem
    }
    public ConditionType type;
    [Header("Trust Level")]
    public int upperBound;
    public int lowerBound;
    public int npcId;
    [Header("Previous Line Played")]
    public string lineId;
    [Header("Money")]
    public int threshold;
    public bool greaterThan;
    [Header("Has Item")]
    public Item item;
    public bool invert;
    

    public bool Evaluate(GameManager gameManager)
    {
        if (type == ConditionType.TrustLevel)
        {
            NPCData nPCData = gameManager.npcMemory[npcId];
            if (nPCData == null) return false;
            int comparedValue = nPCData.trust;
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
        } else if (type == ConditionType.HasItem)
        {
            return gameManager.items.Contains(item) ^ invert;
        }
        return false;
    }
}
