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
        if (gameManager.npcMemory.Capacity <= data.id || gameManager.npcMemory[data.id].name != data.name) gameManager.npcMemory.Insert(data.id, data);
    }

    public void UpdateKindness(int effect)
    {
        int index = gameManager.npcMemory.IndexOf(data);
        data.kindness += effect;
        gameManager.npcMemory[index].kindness = data.kindness;
    }
}
