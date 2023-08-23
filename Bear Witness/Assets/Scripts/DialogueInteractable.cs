using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueInteractable : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;
    public Animator animator;
    public TextMeshProUGUI text;
    public string interactText = "LOOK";

    private void Start()
    {
        text.text = interactText;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player") 
        {
            animator.SetBool("playerInRange", true);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                dialogueTrigger.TriggerDialogue();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            animator.SetBool("playerInRange", false);
        }
    }
}
