using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "NPC")]
public class NPC : ScriptableObject
{
    public string name;
    public RuntimeAnimatorController faceAnimations;
    public string[] emotionReference;
    public Sprite neutralFace;
    public Sprite dialogueBox;

    public int GetEmotionReference(string emotion)
    {
        int target = Array.IndexOf(emotionReference, emotion);
        return target;
    }
}
