using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueControllerByLoop
{
    public TextAsset dialogue;
    public bool periodic = false;
    public int loopNumber;
    public int offset;
    public bool excludeZeroLoop = true;
}
