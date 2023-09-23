using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI choiceA;
    public TextMeshProUGUI choiceB;
    public Image faceDisplay;
    public Animator animator;

    private bool dialogueRunning = false;
    private int frameDelay = 20;

    private Queue<DialogueSentence> sentences;

    private float choiceSelected = 0;
    private DialogueSentence currentSentence;
    private GameManager gameManager;

    private UnityEvent DialogueEndEvent;

    void Awake()
    {
        sentences = new Queue<DialogueSentence>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (frameDelay > 0)
        {
            frameDelay -= 1;
        }
        else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) && dialogueRunning)
        {
            if (currentSentence.isChoice)
            {
                if (choiceSelected == 0) currentSentence.ChooseAEvent.Invoke();
                if (choiceSelected == 1) currentSentence.ChooseBEvent.Invoke();
            }
            DisplayNextSentence();
        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            choiceSelected += Input.GetAxisRaw("Horizontal");
            choiceSelected = Mathf.Abs(choiceSelected % 2);
            HighlightSelection();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        choiceA.text = "";
        choiceB.text = "";
        dialogueText.text = "";
        DialogueSentence[] dialogueElements = dialogue.elements;
        frameDelay = 20;
        if (!dialogueRunning)
        {
            animator.SetBool("IsOpen", true);

            sentences.Clear();

            DialogueEndEvent = dialogue.DialogueEndEvent;
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
            EndDialogue();
            return;
        }
        currentSentence = sentences.Dequeue();
        if ((!currentSentence.hasCondition || currentSentence.condition.Evaluate(gameManager)) && (!gameManager.playedLines.Contains(currentSentence.lineId) || !currentSentence.singleUse))
        {
            if (currentSentence.singleUse) gameManager.playedLines.Add(currentSentence.lineId);
            WriteToDialogueBox();
        } else 
        {
            DisplayNextSentence();
        }
    }

    void WriteToDialogueBox()
    {
        Sprite face = currentSentence.face;
        string name = currentSentence.name;
        string sentence = currentSentence.sentenceText;
        StopAllCoroutines();
        if (currentSentence.isChoice)
        {
            string choiceAtext = currentSentence.choiceTextA;
            string choiceBtext = currentSentence.choiceTextB;
            StartCoroutine(TypeSentence(choiceAtext, choiceA));
            StartCoroutine(TypeSentence(choiceBtext, choiceB));
        }
        else
        {
            choiceA.text = "";
            choiceB.text = "";
        }
        faceDisplay.sprite = face;
        nameText.text = name;
        StartCoroutine(TypeSentence(sentence, dialogueText));
    }

    public void HighlightSelection()
    {
        if (choiceSelected == 0)
        {
            choiceA.color = Color.yellow;
            choiceB.color = Color.white;
        } else if (choiceSelected == 1)
        {
            choiceA.color = Color.white;
            choiceB.color = Color.yellow;
        }
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
                timeDelay = 0.25f;
            } else if (letter == ',' || letter == ':')
            {
                timeDelay = 0.125f;
            }
            yield return new WaitForSeconds(timeDelay);
        }
    }

    void EndDialogue()
    {
        StartCoroutine(DialogueEndDelay());
        animator.SetBool("IsOpen", false);
    }

    IEnumerator DialogueEndDelay()
    {
        yield return new WaitForSeconds(0.5f);
        dialogueRunning = false;
        FindObjectOfType<PlayerMovement>().frozen = false;
        DialogueEndEvent.Invoke();
    }
}
