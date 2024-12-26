using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEvolveOverLoops : MonoBehaviour
{
    [Header("Priority top to bottom")]
    public List<DialogueControllerByLoop> stateMachines = new();
    private int loopNumber = 0;
    public Animator targetAnimator;
    public RuntimeAnimatorController defaultController;

    public NPCController npcController;

    public string setLevelMusicIfPresent;

    public bool disableIfNotPresent;

    private void Start()
    {
        loopNumber = GameManager.instance.loopNumber;

        foreach (DialogueControllerByLoop stateMachine in stateMachines)
        {
            if (loopNumber == stateMachine.loopNumber + stateMachine.offset)
            {
                SetDialogue(stateMachine);
                return;
            } else if (stateMachine.periodic && (loopNumber - stateMachine.offset) % stateMachine.loopNumber == 0)
            {
                if (!(loopNumber == 0 && stateMachine.excludeZeroLoop))
                {
                    SetDialogue(stateMachine);
                    return;
                }
            }
        }

        Debug.Log("No special cases found");
        npcController.enabled = false;
        targetAnimator.runtimeAnimatorController = defaultController;
        if (disableIfNotPresent)
        {
            gameObject.SetActive(false);
        }
    }

    void SetDialogue(DialogueControllerByLoop stateMachine)
    {
        targetAnimator.runtimeAnimatorController = stateMachine.stateMachine;
        if (setLevelMusicIfPresent != "")
        {
            FindObjectOfType<LevelLoader>().overrideLevelMusic = setLevelMusicIfPresent;            
        }
    }
}
