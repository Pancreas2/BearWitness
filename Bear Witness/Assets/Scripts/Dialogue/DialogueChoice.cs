using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    [TextArea(3, 10)]
    public string text;
    public bool hasCondition;
    public int choiceID;
    public List<DialogueCondition> conditions;

    public bool EvaluateConditions(GameManager gameManager)
    {
        List<bool> results = new();
        foreach (DialogueCondition condition in conditions)
        {
            if (results.Count <= condition.conditionSet) results.Insert(condition.conditionSet, true);
            results[condition.conditionSet] = results[condition.conditionSet] && condition.EvaluateCondition(gameManager);
        }
        foreach (bool result in results)
        {
            if (result) return true;
        }
        return false;
    }
}
