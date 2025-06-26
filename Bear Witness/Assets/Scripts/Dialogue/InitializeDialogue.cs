using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeDialogue : StateMachineBehaviour
{
    GameManager gameManager;
    public NPC targetNPC;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        NPCData npcData = gameManager.npcMemory.Find(npcData => npcData.npc == targetNPC.name);
        if (npcData == null)
        {
            Debug.Log("Adding NPC data for " + targetNPC.name);
            NPCData newData = new();
            newData.npc = targetNPC.name;
            newData.displayName = targetNPC.name;
            newData.trust = 0;
            gameManager.npcMemory.Add(newData);
            npcData = newData;
        }

        animator.SetInteger("Trust", npcData.trust);
        animator.SetBool("Met", npcData.met);
        animator.SetBool("SpokenTo", npcData.spokenTo);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        NPCData npcData = gameManager.npcMemory.Find(npcData => npcData.npc == targetNPC.name);
        if (npcData == null)
        {
            Debug.LogError("NPC " + targetNPC.name + " not found");
        }
        int index = gameManager.npcMemory.IndexOf(npcData);
        npcData.spokenTo = true;
        gameManager.npcMemory[index] = npcData;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
