using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateCall : MonoBehaviour
{
    public NPC target;
    private CallManager callManager;
    public Animator callDialogueStateMachine;

    public void Call()
    {
        callManager = FindObjectOfType<CallManager>();

        callManager.currentDialogueStateMachine = callDialogueStateMachine;
        callDialogueStateMachine.SetTrigger("StartDialogue");
    }
}
