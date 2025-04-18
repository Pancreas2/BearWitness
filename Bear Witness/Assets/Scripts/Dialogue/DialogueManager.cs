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
    public Animator faceAnimator;

    public Image dialogueBox;

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

    public GameObject shopMenu;

    private bool inShop = false;
    private int shopPageIndex;
    [SerializeField] private List<ShopDisplay> shopDisplays = new();

    private GameObject lastSelected;

    void Start()
    {
        sentences = new Queue<DialogueSentence>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
        shopMenu.SetActive(false);

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

        if (dialogueRunning)
        {
            // code that keeps the cursor on one of the dialogue options

            if (currentSentence.isChoice)
            {
                bool choiceSelected = false;
                foreach (TextMeshProUGUI choice in choices)
                {
                    if (choice.gameObject == lastSelected) choiceSelected = true;
                }

                Debug.Log(lastSelected);

                if (!choiceSelected)
                {
                    EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
                }
            }

            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
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
        } else
        {
            currentSentence = sentences.Dequeue();

            if ((!currentSentence.hasCondition || currentSentence.EvaluateConditions(gameManager)) && (!gameManager.playedLines.Contains(currentSentence.lineId) || !currentSentence.singleUse))
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
        animator.SetBool("Proceed", false);
        animator.SetBool("LastState", false);
        NPCData speaker = gameManager.npcMemory.Find(npcData => npcData.npc.name == currentSentence.speaker.name);
        faceDisplay.sprite = currentSentence.speaker.neutralFace;
        dialogueBox.sprite = currentSentence.speaker.dialogueBox;
        faceAnimator.runtimeAnimatorController = currentSentence.speaker.faceAnimations;
        faceAnimator.SetInteger("State", currentSentence.speaker.GetEmotionReference(currentSentence.emotion));
        string name = "";
        if (speaker != null)
        {
            if (speaker.displayName == "")
            {
                speaker.displayName = speaker.npc.name;
            }

            if (speaker.displayName != "Narrator")
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
                    } else
                    {
                        j++;
                    }
                }
                else
                {
                    choices[i - j].gameObject.SetActive(false);
                }
            }

            // auto-selects the top choice
            lastSelected = choices[0].gameObject;
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

            if (letter == '(')
            {
                faceAnimator.ResetTrigger("Start");
                faceAnimator.SetTrigger("Stop");
            }

            if (letter == ')')
            {
                faceAnimator.ResetTrigger("Stop");
                faceAnimator.SetTrigger("Start");
            }

            if (letter == '|')
            {
                writingDialogueCommand = true;
                timeDelay = 0f;
            } else if (writingDialogueCommand)
            {
                switch (letter)
                {
                    case '0':
                        timeDelay = 0.25f;
                        break;
                    case '1':
                        timeDelay = 0.5f;
                        break;
                    case 'P':
                        timeDelay = 3f;
                        break;
                    case 'B':
                        writingDialogueCommand = false;
                        DisplayNextSentence();
                        break;
                }
                writingDialogueCommand = false;
            } else
            {
                destination.text += letter;
            }

            if (timeDelay > 1f)
            {
                faceAnimator.ResetTrigger("Start");
                faceAnimator.SetTrigger("Stop");
            }

            yield return new WaitForSeconds(timeDelay);

            if (timeDelay > 1f)
            {
                faceAnimator.ResetTrigger("Stop");
                faceAnimator.SetTrigger("Start");
            }
        }
        if (!inShop)
        {
            if (lastState && sentences.Count < 1)
                animator.SetBool("LastState", true);
            else
                animator.SetBool("Proceed", true);
        }
        faceAnimator.ResetTrigger("Start");
        faceAnimator.SetTrigger("Stop");
    }

    public void EndDialogue()
    {
        gameManager.pauseGameTime = false;
        animator.SetBool("IsOpen", false);
        dialogueRunning = false;

        PlayerMovement playerM = FindObjectOfType<PlayerMovement>();
        playerM.ClearInputs();

        if (!currentDialogueStateMachine)
            playerM.frozen = false;
        if (OnDialogueEnd != null)
            OnDialogueEnd.Invoke();
    }

    public void RefreshShop(ShopData shopData)
    {
        shopMenu.SetActive(true);
        inShop = true;
        EventSystem.current.SetSelectedGameObject(shopDisplays[0].gameObject);

        for (int i = 0; i < 6; i++)
        {
            if (shopData.stock.Count - 1 < i + shopPageIndex * 6)
            {
                // turns off shop display if no item is held
                shopDisplays[i].gameObject.SetActive(false);
            }
            else
            {
                shopDisplays[i].gameObject.SetActive(true);
                ShopItem currentItem;
                currentItem = shopData.stock[shopPageIndex * 6 + i];
                shopDisplays[i].SetItem(currentItem, false, shopPageIndex * 6 + i);
            }
            shopDisplays[i].SetShop(shopData);
        }
    }

    public void ExitShop()
    {
        inShop = false;
        shopMenu.SetActive(false);
        currentDialogueStateMachine.SetTrigger("ExitShop");
    }
}
