using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image faceDisplay;
    public Animator animator;

    private Queue<string> sentences;
    private Queue<string> names;
    private Queue<Sprite> faces;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        faces = new Queue<Sprite>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        FindObjectOfType<PlayerController>().Freeze();
        animator.SetBool("IsOpen", true);

        sentences.Clear();
        names.Clear();
        faces.Clear();

        foreach (string sentence in dialogue.sentences) {
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
        FindObjectOfType<PlayerController>().Unfreeze();
        animator.SetBool("IsOpen", false);
    }
}
