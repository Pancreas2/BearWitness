using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : StateMachineBehaviour
{
    GameManager gameManager;

    public enum EventType
    {
        RemoveItem,
        OpenDoor,
        DestroySelf
    }

    public EventType eventType;

    [Header("Remove Item")]
    public Item targetItem;

    [Header("Open Door")]
    public int doorId;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        switch (eventType)
        {
            case EventType.RemoveItem:
                if (targetItem.type == Item.ItemType.Item)
                {
                    gameManager.items.Remove(targetItem);
                }
                else if (targetItem.type == Item.ItemType.Tool)
                {
                    gameManager.tools.Remove(targetItem);
                }
                break;

            case EventType.OpenDoor:
                gameManager.doorStates[doorId] = true;
                break;

            case EventType.DestroySelf:
                animator.gameObject.SetActive(false);
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
