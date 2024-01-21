using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueBranch : StateMachineBehaviour
{
    private DialogueManager dialogueManager;
    private GameManager gameManager;
    public Dialogue dialogueSegment;
    public bool lastState = false;
    public string triggerOnEnd;
    public Item consumeOnEnd;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        gameManager = FindObjectOfType<GameManager>();
        animator.ResetTrigger("Choose");
        animator.SetInteger("Choice", -1);
        base.OnStateEnter(animator, stateInfo, layerIndex);
        dialogueManager.dialogueRunning = false;
        dialogueManager.currentDialogueStateMachine = animator;
        dialogueManager.StartDialogue(dialogueSegment);
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        animator.SetTrigger(triggerOnEnd);

        if (consumeOnEnd && gameManager.items.Contains(consumeOnEnd))
        {
            gameManager.items.Remove(consumeOnEnd);
        }

        if (lastState)
        {
            dialogueManager.EndDialogue();
            FindObjectOfType<PlayerMovement>().frozen = false;
        }
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
