using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CallManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public GameObject choiceTexts;
    private List<TextMeshProUGUI> choices = new();

    [SerializeField] private GameObject templateChoice;

    public Image faceDisplay;
    public Animator faceAnimator;

    public Animator animator;

    private AudioManager audioManager;
    [HideInInspector] public bool dialogueRunning = false;
    private int frameDelay = 20;

    private Queue<DialogueSentence> sentences;

    private DialogueSentence currentSentence;
    private GameManager gameManager;

    public Animator currentDialogueStateMachine;

    public UnityEvent OnDialogueEnd;
    public bool lastState = false;

    private bool writingDialogueCommand;

    private List<NPC> numbers = new();

    void Start()
    {
        sentences = new Queue<DialogueSentence>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();

        for (int i = 0; i < 6; i++)
        {
            GameObject newChoice = Instantiate(templateChoice, choiceTexts.transform, false);
            newChoice.transform.localPosition = new Vector3(Mathf.Floor(i / 3) * 200f, i % 3 * -30f, 0);
            choices.Add(newChoice.GetComponent<TextMeshProUGUI>());
        }
    }

    void Update()
    {

        if (frameDelay > 0)
        {
            frameDelay -= 1;
        }
        else if (Input.GetButtonDown("Jump") && dialogueRunning)
        {
            if (currentSentence.isChoice)
            {
                GameObject selected = EventSystem.current.currentSelectedGameObject;
                if (currentDialogueStateMachine != null)
                {
                    for (int i = 0; i < choices.Count; i++)
                    {
                        if (choices[i].gameObject == selected)
                        {
                            currentDialogueStateMachine.SetInteger("Choice", choices[i].GetComponent<DialogueOption>().choice);
                        }
                    }
                }
            }

            DisplayNextSentence();
        }
    }

    private void LateUpdate()
    {
        faceDisplay.SetNativeSize();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        List<DialogueSentence> dialogueElements = dialogue.elements;
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

            gameManager.pauseGameTime = true;

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
        }
        else
        {
            currentSentence = sentences.Dequeue();

            if ((!currentSentence.hasCondition || currentSentence.EvaluateConditions(gameManager)) && (!gameManager.playedLines.Contains(currentSentence.lineId) || !currentSentence.singleUse))
            {
                if (currentSentence.singleUse) gameManager.playedLines.Add(currentSentence.lineId);
                audioManager.Play("Select");
                WriteToDialogueBox();
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    void WriteToDialogueBox()
    {
        animator.SetBool("Proceed", false);
        animator.SetBool("LastState", false);
        NPCData speaker = gameManager.npcMemory.Find(npcData => npcData.npc == currentSentence.speaker.name);
        faceDisplay.sprite = currentSentence.speaker.neutralFace;
        faceAnimator.runtimeAnimatorController = currentSentence.speaker.faceAnimations;
        faceAnimator.SetInteger("State", currentSentence.speaker.GetEmotionReference(currentSentence.emotion));
        string name = "";
        if (speaker != null)
        {
            if (speaker.displayName == "")
            {
                speaker.displayName = speaker.npc;
            }
            name = speaker.displayName;
        }
        string sentence = currentSentence.sentenceText;
        StopAllCoroutines();
        if (currentSentence.isChoice)
        {
            nameText.text = "";
            StartCoroutine(TypeSentence(sentence, dialogueText));

            bool chosenInitialSelected = false;
            int j = 0;
            for (int i = 0; i - j < choices.Count; i++)
            {
                Debug.Log(i + ", " + j);
                if (i < currentSentence.choices.Count)
                {
                    if (!currentSentence.choices[i].hasCondition || currentSentence.choices[i].EvaluateConditions(gameManager))
                    {
                        choices[i - j].gameObject.SetActive(true);
                        if (!chosenInitialSelected)
                        {
                            chosenInitialSelected = true;
                            EventSystem.current.SetSelectedGameObject(choices[i - j].gameObject);
                        }
                        StartCoroutine(TypeSentence(currentSentence.choices[i].text, choices[i - j]));

                        choices[i - j].GetComponent<DialogueOption>().choice = currentSentence.choices[i].choiceID;
                    }
                    else
                    {
                        j++;
                    }
                }
                else
                {
                    choices[i - j].gameObject.SetActive(false);
                }
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

    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI destination)
    {
        faceAnimator.ResetTrigger("Stop");
        faceAnimator.SetTrigger("Start");
        destination.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            float timeDelay = 0.015f;

            if (letter == '|')
            {
                writingDialogueCommand = true;
                timeDelay = 0f;
            }
            else if (writingDialogueCommand)
            {
                switch (letter)
                {
                    case '0':
                        timeDelay = 0.25f;
                        break;
                    case '1':
                        timeDelay = 0.5f;
                        break;
                }
                writingDialogueCommand = false;
            }
            else
            {
                destination.text += letter;
            }

            yield return new WaitForSecondsRealtime(timeDelay);
        }
        if (lastState && sentences.Count < 1)
            animator.SetBool("LastState", true);
        else
            animator.SetBool("Proceed", true);

        faceAnimator.ResetTrigger("Start");
        faceAnimator.SetTrigger("Stop");
    }

    public void EndDialogue()
    {
        gameManager.pauseGameTime = false;
        animator.SetBool("IsOpen", false);
        dialogueRunning = false;
        if (!currentDialogueStateMachine)
            FindObjectOfType<PlayerMovement>().frozen = false;
        if (OnDialogueEnd != null)
            OnDialogueEnd.Invoke();
    }

    public void AddNumber(NPC npc)
    {
        numbers.Add(npc);
    }
}
