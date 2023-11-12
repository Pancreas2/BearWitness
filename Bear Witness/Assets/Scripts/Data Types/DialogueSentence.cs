using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueSentence {
    public string lineId;
    public bool singleUse = false;
    public bool hasCondition = false;
    public DialogueCondition condition;
    [Header("Dialogue")]
    public NPC speaker;
    public string emotion;
    [TextArea(3, 10)]
    public string sentenceText;

    [Header("Choices")]
    
    public bool isChoice;
    [TextArea(3, 10)]
    public string[] choices;

    public bool isHub;
}
