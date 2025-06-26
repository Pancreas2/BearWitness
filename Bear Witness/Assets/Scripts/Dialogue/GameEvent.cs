using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvent : StateMachineBehaviour
{
    GameManager gameManager;

    public enum EventType
    {
        RemoveItem,
        OpenDoor,
        DestroySelf,
        DestroyDialogue
    }

    public EventType eventType;

    [Header("Remove Item")]
    public Item targetItem;

    [Header("Open Door")]
    public Gate.Gates doorName;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();

        base.OnStateEnter(animator, stateInfo, layerIndex);
        switch (eventType)
        {
            case EventType.RemoveItem:
                if (targetItem.type == Item.ItemType.Item)
                {
                    gameManager.items.Remove(targetItem.name);
                }
                else if (targetItem.type == Item.ItemType.Tool)
                {
                    gameManager.tools.Remove(targetItem.name);
                }
                break;

            case EventType.OpenDoor:
                gameManager.doorStates[Gate.GateMatch[doorName]] = true;
                break;

            case EventType.DestroySelf:
                animator.gameObject.SetActive(false);
                break;

            case EventType.DestroyDialogue:
                animator.gameObject.GetComponent<DialogueInteractable>().enabled = false;
                break;
        }
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
