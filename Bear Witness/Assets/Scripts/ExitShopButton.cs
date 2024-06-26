using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitShopButton : MonoBehaviour
{
    DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void OnClick()
    {
        dialogueManager.ExitShop();
    }
}
