using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Ink.Runtime;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

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
    private bool doneSpeaking = false;
    private int positionTracker = 0;

    public GameObject shopMenu;

    private bool inShop = false;
    private int shopPageIndex;
    [SerializeField] private List<ShopDisplay> shopDisplays = new();

    private GameObject lastSelected;

    // Ink stuff:
    private Story dialogue;
    private bool choiceAvailable = false;

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
            if (choiceAvailable)
            {
                choiceAvailable = false;
                GameObject selected = EventSystem.current.currentSelectedGameObject;
                for (int i = 0; i < choices.Count; i++)
                {
                    if (choices[i].gameObject == selected)
                    {
                        dialogue.ChooseChoiceIndex(i);
                    }
                }

                DisplayNextSentence();

            } else
            {
                if (doneSpeaking || inShop)
                    DisplayNextSentence();
                else SkipToLineEnd(dialogue.currentText, dialogueText);
            }
        }

        if (dialogueRunning)
        {
            // code that keeps the cursor on one of the dialogue options

            if (dialogue.currentChoices.Count > 0)
            {
                bool choiceSelected = false;
                foreach (TextMeshProUGUI choice in choices)
                {
                    if (choice.gameObject == lastSelected) choiceSelected = true;
                }

                if (!choiceSelected)
                {
                    EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
                }
            }

            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
    }

    public void PlayLine(string line)
    {
        dialogue.ChoosePathString(line);
    }

    public void StartDialogue(TextAsset inputDialogue)
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();

        frameDelay = 20;
        if (!dialogueRunning)
        {

            if (!inputDialogue)
            {
                inputDialogue = Resources.Load<TextAsset>("default_dialogue");
            }

            dialogue = new Story(inputDialogue.text);
            string dialogueID = dialogue.globalTags[0];

            dialogue.BindExternalFunction("changeFriend", (string id, int modifier) =>
            {
                gameManager.npcMemory.Find(target => target.id == id).trust += modifier;
            });

            dialogue.BindExternalFunction("changeMoney", (int modifier) =>
            {
                gameManager.money += modifier;
            });

            dialogue.BindExternalFunction("changeName", (string id, string name) =>
            {
                gameManager.npcMemory.Find(target => target.id == id).displayName = name;
            });

            dialogue.BindExternalFunction("giveItem", (string id) =>
            {
                gameManager.PickupItem(Resources.Load<Item>(id));
            });

            dialogue.BindExternalFunction("hasItem", (string id) =>
            {
                return gameManager.items.Contains(id);
            });

            dialogue.BindExternalFunction("openDoor", (string id) =>
            {
                gameManager.doorStates[Gate.GateMatch[Gate.stringToGate(id)]] = true;
            });

            dialogue.BindExternalFunction("openShop", (string id) =>
            {
                FindObjectOfType<WalletUI>().inShop = true;
                ShopData currentData = gameManager.shopMemory.Find(npcShop => npcShop.name == id);

                if (currentData == null)
                {
                    Debug.Log("Adding shop data for " + id);
                    ShopData newData = new();
                    newData.LoadFromBaseShop(Resources.Load<NPCShop>("Shops/" + id));
                    gameManager.shopMemory.Add(newData);
                    RefreshShop(newData);
                } else
                {
                    RefreshShop(currentData);
                }
            });

            // the first line of ink text is always skipped to separate global tags and save flags
            if (gameManager.currentStories.ContainsKey(dialogueID))
            {
                dialogue.state.LoadJson(gameManager.currentStories[dialogueID]);
                PlayLine("start");
                dialogue.Continue();
            } else
            {
                dialogue.Continue();
            }

            if (dialogue.variablesState.GlobalVariableExistsWithName("money")) dialogue.variablesState["money"] = gameManager.money;
            if (dialogue.variablesState.GlobalVariableExistsWithName("friend")) dialogue.variablesState["friend"] = gameManager.npcMemory.Find(target => target.id == dialogueID[0..3]).trust;
            if (dialogue.variablesState.GlobalVariableExistsWithName("loops")) dialogue.variablesState["loops"] = gameManager.loopNumber;
            if (dialogue.variablesState.GlobalVariableExistsWithName("plot_progress")) dialogue.variablesState["plot_progress"] = gameManager.plotProgress;
            if (dialogue.variablesState.GlobalVariableExistsWithName("time")) dialogue.variablesState["time"] = gameManager.gameTime;

            foreach (TextMeshProUGUI choice in choices)
            {
                choice.text = "";
            }

            dialogueText.text = "";

            animator.SetBool("IsOpen", true);

            sentences.Clear();

            gameManager.pauseGameTime = true;

            DisplayNextSentence();
            dialogueRunning = true;
        }
    }

    public void DisplayNextSentence()
    {
        doneSpeaking = false;
        if (dialogue.canContinue)
        {
            string sentence = dialogue.Continue();
            string tag = "_";
            if (dialogue.currentTags.Count > 0)
            {
                faceDisplay.gameObject.SetActive(true);
                tag = dialogue.currentTags[0];
                string id = tag[0..tag.IndexOf("_")];
                NPCData speaker = gameManager.npcMemory.Find(target => target.id == id);

                NPC npc = Resources.Load<NPC>("NPCs/" + speaker.npc);
                faceAnimator.runtimeAnimatorController = npc.faceAnimations;
                faceAnimator.SetInteger("State", npc.GetEmotionReference(tag));

                nameText.text = speaker.displayName;
            } else
            {
                faceDisplay.gameObject.SetActive(false);
            }

            StopAllCoroutines();

            foreach (TextMeshProUGUI choice in choices)
            {
                choice.text = "";
                choice.gameObject.SetActive(false);
            }

            StartCoroutine(TypeSentence(sentence, dialogueText));
        } else
        {
            if (dialogue.currentChoices.Count > 0)
            {
                faceDisplay.gameObject.SetActive(false);
                StopAllCoroutines();
                // choice
                nameText.text = "";
                dialogueText.text = "";

                for (int i = 0; i < choices.Count; i++)
                {
                    if (i < dialogue.currentChoices.Count)
                    {
                        choices[i].gameObject.SetActive(true);
                        StartCoroutine(TypeSentence(dialogue.currentChoices[i].text, choices[i]));
                    }
                    else
                    {
                        choices[i].gameObject.SetActive(false);
                    }
                }

                // auto-selects the top choice
                EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
                lastSelected = choices[0].gameObject;
                choiceAvailable = true;
            }
            else EndDialogue();
        }
    }

    void WriteToDialogueBox()
    {
        animator.SetBool("Proceed", false);
        animator.SetBool("LastState", false);
        NPCData speaker = gameManager.npcMemory.Find(npcData => npcData.npc == currentSentence.speaker.name);
        faceDisplay.sprite = currentSentence.speaker.neutralFace;
        dialogueBox.sprite = currentSentence.speaker.dialogueBox;
        faceAnimator.runtimeAnimatorController = currentSentence.speaker.faceAnimations;
        faceAnimator.SetInteger("State", currentSentence.speaker.GetEmotionReference(currentSentence.emotion));
        string name = "";
        if (speaker != null)
        {
            if (speaker.displayName == "")
            {
                speaker.displayName = speaker.npc;
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

    void SkipToLineEnd(string sentence, TextMeshProUGUI destination)
    {
        StopAllCoroutines();
        faceAnimator.ResetTrigger("Start");
        faceAnimator.SetTrigger("Stop");
        string leftover = sentence.Substring(positionTracker + 1);
        foreach (char letter in leftover.ToCharArray())
        {
            if (letter == '^')
            {
                writingDialogueCommand = true;
            } else if (!writingDialogueCommand && letter != '`')
            {
                destination.text += letter;
            } else if (writingDialogueCommand)
            {
                writingDialogueCommand = false;
            }
        }
        doneSpeaking = true;
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI destination)
    {
        faceAnimator.ResetTrigger("Stop");
        faceAnimator.SetTrigger("Start");
        destination.text = "";
        positionTracker = 0;
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
                // causes a 'hiccup' on the face animator, fix later
                faceAnimator.ResetTrigger("Stop");
                faceAnimator.SetTrigger("Start");
            }

            if (letter == '^')
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
                    case 'S':
                        string dialogueID = dialogue.globalTags[0];
                        if (gameManager.currentStories.ContainsKey(dialogueID))
                        {
                            gameManager.currentStories[dialogueID] = dialogue.state.ToJson();
                        }
                        else
                        {
                            gameManager.currentStories.Add(dialogueID, dialogue.state.ToJson());
                        }
                        EndDialogue();
                        break;
                }
                writingDialogueCommand = false;
            } else if (letter != '`')
            {
                destination.text += letter;
            }

            if (timeDelay > 1f)
            {
                faceAnimator.ResetTrigger("Start");
                faceAnimator.SetTrigger("Stop");
            }

            yield return new WaitForSeconds(timeDelay);
            positionTracker++;

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
        doneSpeaking = true;
    }

    public void EndDialogue()
    {
        gameManager.pauseGameTime = false;
        animator.SetBool("IsOpen", false);
        dialogueRunning = false;

        PlayerMovement playerM = FindObjectOfType<PlayerMovement>();
        playerM.ClearInputs();

        playerM.Unfreeze("Dialogue");
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
        PlayLine("exit");
    }
}
