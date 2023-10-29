using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueInteractable : Interactable
{
    public Animator dialogueStateMachine;
    public DialogueTrigger dialogueTrigger;

    override public void OnInteract()
    {
        if (dialogueStateMachine == null || dialogueStateMachine.GetCurrentAnimatorStateInfo(0).IsName("nothing"))
        {
            if (dialogueStateMachine != null)
                dialogueStateMachine.SetTrigger("StartDialogue");
            else
                dialogueTrigger.TriggerDialogue();
            FindObjectOfType<PlayerMovement>().frozen = true;
        }
    }
}
