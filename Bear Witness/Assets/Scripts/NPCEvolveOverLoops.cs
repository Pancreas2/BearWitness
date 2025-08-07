using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEvolveOverLoops : MonoBehaviour
{
    [Header("Priority top to bottom")]
    public List<DialogueControllerByLoop> dialogues = new();
    private int loopNumber = 0;
    public DialogueInteractable targetInteractable;
    public TextAsset defaultDialogue;

    public string setLevelMusicIfPresent;

    public bool disableIfNotPresent;

    private void Start()
    {
        loopNumber = GameManager.instance.loopNumber;

        foreach (DialogueControllerByLoop controller in dialogues)
        {
            if (loopNumber == controller.loopNumber + controller.offset)
            {
                SetDialogue(controller);
                return;
            } else if (controller.periodic && (loopNumber - controller.offset) % controller.loopNumber == 0)
            {
                if (!(loopNumber == 0 && controller.excludeZeroLoop))
                {
                    SetDialogue(controller);
                    return;
                }
            }
        }

        Debug.Log("No special cases found");
        targetInteractable.enabled = false;
        targetInteractable.dialogue = defaultDialogue;
        if (disableIfNotPresent)
        {
            gameObject.SetActive(false);
        }
    }

    void SetDialogue(DialogueControllerByLoop controller)
    {
        targetInteractable.dialogue = controller.dialogue;
        if (setLevelMusicIfPresent != "")
        {
            FindObjectOfType<LevelLoader>().overrideLevelMusic = setLevelMusicIfPresent;            
        }
    }
}
