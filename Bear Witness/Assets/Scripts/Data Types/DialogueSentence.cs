using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueSentence {
    public string lineId;
    public bool singleUse = false;
    [Header("Conditions")]
    public bool hasCondition = false;
    public List<DialogueCondition> conditions = new();
    [Header("Dialogue")]
    public NPC speaker;
    public string emotion;
    [TextArea(3, 10)]
    public string sentenceText;

    [Header("Choices")]
    
    public bool isChoice;
    public List<DialogueChoice> choices = new();

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
