using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public GameObject choiceTexts;
    private List<TextMeshProUGUI> choices = new();

    [SerializeField] private GameObject templateChoice;

    public Image faceDisplay;
    public Animator animator;

    public DialogueButtons buttons;

    private AudioManager audioManager;
    [HideInInspector] public bool dialogueRunning = false;
    private int frameDelay = 20;

    private Queue<DialogueSentence> sentences;

    private DialogueSentence currentSentence;
    private GameManager gameManager;

    public Animator currentDialogueStateMachine;

    public UnityEvent OnDialogueEnd;

    void Start()
    {
        sentences = new Queue<DialogueSentence>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();

        for (int i = 0; i < 6; i++)
        {
            GameObject newChoice = Instantiate(templateChoice, choiceTexts.transform, false);
            newChoice.transform.localPosition = new Vector3(Mathf.Floor(i / 3) * 0.5f, i % 3 * -0.25f, 0);
            choices.Add(newChoice.GetComponent<TextMeshProUGUI>());
        }
    }

    void Update()
    {
        if (frameDelay > 0)
        {
            frameDelay -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && dialogueRunning)
        {

            if (currentSentence.isHub)
            {
                currentDialogueStateMachine.SetInteger("Choice", buttons.GetButtonCommand());
            }

            if (currentSentence.isChoice)
            {
                GameObject selected = EventSystem.current.currentSelectedGameObject;
                if (currentDialogueStateMachine != null)
                {
                    for (int i = 0; i < choices.Count; i++)
                    {
                        if (choices[i].gameObject == selected)
                        {
                            currentDialogueStateMachine.SetInteger("Choice", 4 + i);
                        }
                    }
                }
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
            foreach (TextMeshProUGUI choice in choices)
            {
                choice.text = "";
            }

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
            if (!currentDialogueStateMachine) EndDialogue();
            else currentDialogueStateMachine.SetTrigger("Choose");
        } else
        {
            currentSentence = sentences.Dequeue();

            if (currentSentence.isHub)
            {
                buttons.SetButtonsActive(true);
                EventSystem.current.SetSelectedGameObject(buttons.talkButton.gameObject);
            }
            else
            {
                buttons.SetButtonsActive(false);
                if (currentSentence.isChoice)
                {
                    EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
                }
            }

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

            for (int i = 0; i < choices.Count; i++)
            {
                if (i < currentSentence.choices.Length)
                {
                    choices[i].gameObject.SetActive(true);
                    StartCoroutine(TypeSentence(currentSentence.choices[i], choices[i]));
                }
                else choices[i].gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (TextMeshProUGUI choice in choices)
            {
                choice.text = "";
                choice.gameObject.SetActive(false);
            }

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
                timeDelay = 0.5f;
            } else if (letter == ',' || letter == ':')
            {
                timeDelay = 0.25f;
            }
            yield return new WaitForSeconds(timeDelay);
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        dialogueRunning = false;
        if (!currentDialogueStateMachine)
            FindObjectOfType<PlayerMovement>().frozen = false;
        if (OnDialogueEnd != null)
            OnDialogueEnd.Invoke();
    }
}
