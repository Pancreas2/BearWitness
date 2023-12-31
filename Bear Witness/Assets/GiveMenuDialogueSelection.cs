using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveMenuDialogueSelection : StateMachineBehaviour
{
    private DialogueManager dialogueManager;
    private GameManager gameManager;
    public Dialogue dialogueSegment;
    public bool lastState = false;
    public string triggerOnEnd;

    public List<Item> acceptedItems;

    public NPC speaker;

    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        gameManager = FindObjectOfType<GameManager>();

        DialogueSentence line = new();
        line.speaker = speaker;

        bool hasGivableItem = false;

        foreach (Item acceptedItem in acceptedItems)
        {
            if (gameManager.items.Contains(acceptedItem))
            {
                line.choices.Add(acceptedItem.name);
                hasGivableItem = true;
            }
            else
                line.choices.Add("");
        }

        if (hasGivableItem)
        {
            line.isChoice = true;
            line.isHub = true;
        }
        else
        {
            line.isChoice = false;
            line.sentenceText = "(I can't think of anything to offer!)";
        }

        dialogueSegment.elements.Add(line);
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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
