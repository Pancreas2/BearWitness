using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    public DialogueSentence[] elements;

    public UnityEvent DialogueEndEvent;
}
