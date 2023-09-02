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
    public string name;
    public Sprite face;
    [TextArea(3, 10)]
    public string sentenceText;
    public bool isChoice;
    public string choiceTextA;
    public string choiceTextB;

    public UnityEvent ChooseAEvent;
    public UnityEvent ChooseBEvent;
}
