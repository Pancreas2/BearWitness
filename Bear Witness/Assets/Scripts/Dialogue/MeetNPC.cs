using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetNPC : StateMachineBehaviour
{
    GameManager gameManager;

    public NPC targetNPC;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        base.OnStateEnter(animator, stateInfo, layerIndex);
        NPCData npcData = gameManager.npcMemory.Find(npcData => npcData.npc == targetNPC.name);
        if (npcData == null)
        {
            Debug.LogAssertion("NPC " + targetNPC.name + " not found");
        }
        int index = gameManager.npcMemory.IndexOf(npcData);
        npcData.met = true;
        gameManager.npcMemory[index] = npcData;
        animator.SetBool("Met", npcData.met);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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
