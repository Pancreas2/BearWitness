using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI choiceA;
    public TextMeshProUGUI choiceB;
    public TextMeshProUGUI choiceC;
    public Image faceDisplay;
    public Animator animator;

    private AudioManager audioManager;
    private bool dialogueRunning = false;
    private int frameDelay = 20;

    private Queue<DialogueSentence> sentences;

    private DialogueSentence currentSentence;
    private GameManager gameManager;

    public Animator currentDialogueStateMachine;

    void Start()
    {
        sentences = new Queue<DialogueSentence>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (frameDelay > 0)
        {
            frameDelay -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && dialogueRunning)
        {
            if (currentSentence.isChoice)
            {
                GameObject selected = EventSystem.current.currentSelectedGameObject;
                if (currentDialogueStateMachine != null)
                {
                    if (selected == choiceA.gameObject)
                    {
                        currentDialogueStateMachine.SetInteger("Choice", 0);
                    } else if (selected == choiceB.gameObject)
                    {
                        currentDialogueStateMachine.SetInteger("Choice", 1);
                    } else if (selected == choiceC.gameObject)
                    {
                        currentDialogueStateMachine.SetInteger("Choice", 2);
                    }
                }

                EventSystem.current.SetSelectedGameObject(choiceA.gameObject);
            }
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        DialogueSentence[] dialogueElements = dialogue.elements;
        frameDelay = 20;
        if (!dialogueRunning)
        {
            choiceA.text = "";
            choiceB.text = "";
            choiceC.text = "";
            dialogueText.text = "";
            animator.SetBool("IsOpen", true);

            sentences.Clear();

            foreach (DialogueSentence element in dialogueElements)
            {
                sentences.Enqueue(element);
            }

            DisplayNextSentence();
            dialogueRunning = true;
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            dialogueRunning = false;
            if (currentDialogueStateMachine != null)
                currentDialogueStateMachine.SetTrigger("Choose");
            else EndDialogue();
            return;
        }
        currentSentence = sentences.Dequeue();
        if ((!currentSentence.hasCondition || currentSentence.condition.Evaluate(gameManager)) && (!gameManager.playedLines.Contains(currentSentence.lineId) || !currentSentence.singleUse))
        {
            if (currentSentence.singleUse) gameManager.playedLines.Add(currentSentence.lineId);
            audioManager.Play("Select");
            WriteToDialogueBox();
        } else 
        {
            DisplayNextSentence();
        }
    }

    void WriteToDialogueBox()
    {
        Sprite face = currentSentence.speaker.GetDialogueFace(currentSentence.emotion).face;
        string name = currentSentence.speaker.name;
        string sentence = currentSentence.sentenceText;
        StopAllCoroutines();
        if (currentSentence.isChoice)
        {
            nameText.text = "";
            dialogueText.text = "";
            string choiceAtext = currentSentence.choiceTextA;
            string choiceBtext = currentSentence.choiceTextB;
            string choiceCtext = currentSentence.choiceTextC;
            StartCoroutine(TypeSentence(choiceAtext, choiceA));
            StartCoroutine(TypeSentence(choiceBtext, choiceB));
            StartCoroutine(TypeSentence(choiceCtext, choiceC));
        }
        else
        {
            choiceA.text = "";
            choiceB.text = "";
            choiceC.text = "";
            nameText.text = name;
            StartCoroutine(TypeSentence(sentence, dialogueText));
        }
        faceDisplay.sprite = face;
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI destination)
    {
        destination.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            destination.text += letter;
            float timeDelay = 0.02f;
            if (letter == '.' || letter == '?' || letter == '!' || letter == ';')
            {
                timeDelay = 1f;
            } else if (letter == ',' || letter == ':')
            {
                timeDelay = 0.5f;
            }
            yield return new WaitForSeconds(timeDelay);
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        dialogueRunning = false;
        if (currentDialogueStateMachine == null)
            FindObjectOfType<PlayerMovement>().frozen = false;
    }
}
