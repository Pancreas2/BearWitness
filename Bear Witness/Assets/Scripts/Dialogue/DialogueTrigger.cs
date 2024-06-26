using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue(float delay = 0f)
    {
        FindObjectOfType<PlayerMovement>().frozen = true;
        StartCoroutine(WaitPatiently(delay));
    }

    IEnumerator WaitPatiently(float delay)
    {
        yield return new WaitForSeconds(delay);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
