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
    public Image faceDisplay;
    public Animator animator;

    private bool dialogueRunning = false;
    private int frameDelay = 20;

    private Queue<string> sentences;
    private Queue<string> names;
    private Queue<Sprite> faces;

    private UnityEvent DialogueEndEvent;
    void Awake()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        faces = new Queue<Sprite>();
    }

    void Update()
    {
        if (frameDelay > 0)
        {
            frameDelay -= 1;
        }
        else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) && dialogueRunning)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        frameDelay = 20;
        if (!dialogueRunning)
        {
            animator.SetBool("IsOpen", true);

            sentences.Clear();
            names.Clear();
            faces.Clear();

            DialogueEndEvent = dialogue.DialogueEndEvent;

            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
            foreach (string name in dialogue.names)
            {
                names.Enqueue(name);
            }
            foreach (Sprite face in dialogue.faces)
            {
                faces.Enqueue(face);
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
        Sprite face = faces.Dequeue();
        string name = names.Dequeue();
        string sentence = sentences.Dequeue();
        faceDisplay.sprite = face;
        nameText.text = name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
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
