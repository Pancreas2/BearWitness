using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DialogueInteractable : Interactable
{
    public Animator dialogueStateMachine;
    public DialogueTrigger dialogueTrigger;

    public UnityEvent OnDialogueStart;
    public UnityEvent OnDialogueEnd;

    private DialogueManager dialogueManager;
    private PlayerMovement playerMovement;

    [SerializeField] private float talkOffset = 0;
    [SerializeField] private bool playerFacesRight = false;

    private bool active = true;

    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    override public void OnInteract()
    {
        if (active && (dialogueStateMachine == null || dialogueStateMachine.GetCurrentAnimatorStateInfo(0).IsName("nothing")))
        {
            if (dialogueStateMachine != null)
                dialogueStateMachine.SetTrigger("StartDialogue");
            else
                dialogueTrigger.TriggerDialogue();

            playerMovement.frozen = true;
            playerMovement.WalkToPoint(transform.position.x + talkOffset);
            playerMovement.cutsceneFaceRight = playerFacesRight;

            dialogueManager.OnDialogueEnd = OnDialogueEnd;
            OnDialogueStart.Invoke();
        }
    }

    public void OnDisable()
    {
        active = false;
        // the animator here is the hover text animator
        base.animator.gameObject.SetActive(false);
    }
}
