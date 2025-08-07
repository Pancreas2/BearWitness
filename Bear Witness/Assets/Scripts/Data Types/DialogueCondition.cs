using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCondition
{
    public enum ConditionType
    {
        None,
        PreviousLinePlayed,
        HasItem
    }
    public ConditionType conditionType;
    public int conditionSet = 0;
    public bool invert;

    [Header("Previous Line Played")]
    public string lineId;

    [Header("Has Item")]
    public Item item;

    public bool EvaluateCondition(GameManager gameManager)
    {
        if (conditionType == ConditionType.PreviousLinePlayed)
        {
            return gameManager.playedLines.Contains(lineId) ^ invert;
        } else if (conditionType == ConditionType.HasItem)
        {
            return (gameManager.ContainsItem(item.name) || gameManager.tools.Contains(item.name)) ^ invert;
        }
        return false;
    }
}
