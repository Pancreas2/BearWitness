using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItem : StateMachineBehaviour
{
    GameManager gameManager;
    WalletUI wallet;

    public Item item;
    public int price;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        gameManager = FindObjectOfType<GameManager>();
        wallet = FindObjectOfType<WalletUI>();
        gameManager.PickupItem(item);
        gameManager.money -= price;
        wallet.AddMoney(-price);
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
