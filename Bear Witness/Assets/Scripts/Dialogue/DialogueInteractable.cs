using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DialogueInteractable : Interactable
{
    public TextAsset dialogue;
    public string startFromNode = "start";

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
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (!dialogue)
        {
            dialogue = Resources.Load<TextAsset>("default_dialogue.json");
        }
    }

    override public void OnInteract()
    {
        if (active)
        {
            OnDialogueStart.Invoke();

            playerMovement.Freeze("Dialogue");
            playerMovement.WalkToPoint(transform.position.x + talkOffset);
            playerMovement.cutsceneFaceRight = playerFacesRight;

            dialogueManager = FindObjectOfType<DialogueManager>();

            dialogueManager.OnDialogueEnd = OnDialogueEnd;

            dialogueManager.StartDialogue(dialogue, startFromNode);
        }
    }

    public void OnDisable()
    {
        active = false;
        // the animator here is the hover text animator
        base.animator.gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        active = true;
        base.animator.gameObject.SetActive(true);
    }
}
