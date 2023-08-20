using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractable : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) 
        {
            dialogueTrigger.TriggerDialogue();
        }
    }


}
