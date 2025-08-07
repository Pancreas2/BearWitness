using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSpecialObject : MonoBehaviour
{
    // FOR OBJECTS THAT ONLY APPEAR FOR CERTAIN CUTSCENES

    [SerializeField] string cutsceneID;

    void Start()
    {
        if (GameManager.instance.playedCutscenes.Contains(cutsceneID)) gameObject.SetActive(false);
    }

    public void MarkCutsceneComplete()
    {
        if (!GameManager.instance.playedCutscenes.Contains(cutsceneID))
        {
            GameManager.instance.playedCutscenes.Add(cutsceneID);
        }
    }
}
