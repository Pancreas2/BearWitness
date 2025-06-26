using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : StateMachineBehaviour
{
    public NPCShop shopType;

    ShopData shopData;
    DialogueManager dialogueManager;
    GameManager gameManager;
    public Dialogue backgroundDialogue;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FindObjectOfType<WalletUI>().inShop = true;
        if (!gameManager) gameManager = GameManager.instance;
        ShopData npcShop = gameManager.shopMemory.Find(npcShop => npcShop.name == shopType.name);
        if (npcShop == null)
        {
            Debug.Log("Adding shop data for " + shopType.name);
            ShopData newData = new();
            foreach(ShopItem item in shopType.startingStock)
            {
                ShopItem newItem = new();
                newItem.stock = item.stock;
                newItem.price = item.price;
                newItem.name = item.name;
                newItem.finiteStock = item.finiteStock;
                newItem.item = item.item;

                newData.stock.Add(newItem);
            }
            newData.backgroundDialogue = shopType.backgroundDialogue;
            newData.name = shopType.name;
            gameManager.shopMemory.Add(newData);
            shopData = newData;
        }
        else
        {
            shopData = npcShop;
        }

        base.OnStateEnter(animator, stateInfo, layerIndex);
        dialogueManager = FindObjectOfType<DialogueManager>();

        dialogueManager.RefreshShop(shopData);
        //dialogueManager.StartDialogue(backgroundDialogue);
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
