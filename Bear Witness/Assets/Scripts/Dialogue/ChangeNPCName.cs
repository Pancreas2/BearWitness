using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeNPCName : StateMachineBehaviour
{
    private GameManager gameManager;
    public NPC targetNPC;
    public string newName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (!gameManager)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        NPCData npcData = gameManager.npcMemory.Find(npcData => npcData.npc == targetNPC.name);
        if (npcData == null)
        {
            Debug.LogAssertion("NPC " + targetNPC.name + " not found");
        }
        int index = gameManager.npcMemory.IndexOf(npcData);

        gameManager.npcMemory[index].displayName = newName;

    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
