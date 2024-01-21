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

        bool containsNPC = false;
        foreach (NPCData npcData in gameManager.npcMemory)
        {
            if (npcData.npc.name == data.npc.name)
            {
                containsNPC = true;
                break;
            }
        }
        
        if (!containsNPC)
        {
            gameManager.npcMemory.Add(data);
        }
    }
}
