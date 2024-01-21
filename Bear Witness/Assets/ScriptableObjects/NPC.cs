using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "NPC")]
public class NPC : ScriptableObject
{
    public string name;
    public DialogueFace[] dialogueFaces;
    public DialogueFace neutralFace;

    public DialogueFace GetDialogueFace(string emotion)
    {
        DialogueFace target = Array.Find(dialogueFaces, face => face.emotion == emotion);
        if (target == null) target = neutralFace;
        return target;
    }
}
