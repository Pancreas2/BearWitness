using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : DialogueInteractable
{
    public NPCData data;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.npcMemory.Insert(data.id, data);
        Debug.Log("added data for " + data.name);
    }

    public void UpdateKindness(int effect)
    {
        Debug.Log("Kindness time");
        int index = gameManager.npcMemory.IndexOf(data);
        data.kindness += effect;
        gameManager.npcMemory[index].kindness = data.kindness;
        Debug.Log(data.name + " kindness updated to " + data.kindness);
    }
}
