using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
    
public class ShopDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    private ShopItem heldItem;
    private ShopData currentShop;
    private GameManager gameManager;
    private DialogueManager dialogueManager;

    private int itemIndex;

    [SerializeField] private PurchaseDisplay purchaseMenu;

    private void Start()
    {
        gameManager = GameManager.instance;
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void SetShop(ShopData newShop)
    {
        currentShop = newShop;
    }

    public void SetItem(ShopItem item, bool purchaseScreen = false, int index = 0)
    {
        heldItem = item;
        itemIndex = index;
        image.sprite = item.item.image;

        if (heldItem.stock < 1 && heldItem.finiteStock)
        {
            if (!purchaseScreen) nameText.text = "OUT OF STOCK";
            priceText.text = "";
        } else
        {
            if (!purchaseScreen) nameText.text = item.name;
            if (item.price == 0) priceText.text = "$ --";
            else priceText.text = "$ " + item.price.ToString();
        }
    }

    public void VerifyPurchase()
    {
        if (heldItem.stock > 0 || !heldItem.finiteStock)
        {
            if (gameManager.money >= heldItem.price)
            {
                purchaseMenu.gameObject.SetActive(true);
                purchaseMenu.SetItem(heldItem, true);
                purchaseMenu.SetShop(currentShop);
                EventSystem.current.SetSelectedGameObject(purchaseMenu.acceptBtn);
                dialogueManager.currentDialogueStateMachine.SetInteger("Choice", itemIndex);
                dialogueManager.currentDialogueStateMachine.SetTrigger("Choose");
            } else
            {
                dialogueManager.currentDialogueStateMachine.SetTrigger("TooExpensive");
            }
        }
    }

    public void PurchaseItem()
    {
        if (heldItem.stock > 0 || !heldItem.finiteStock)
        {
            ShopItem newHeldItem = heldItem;
            newHeldItem.stock--;
            gameManager.PickupItem(heldItem.item);
            gameManager.money -= heldItem.price;
            FindObjectOfType<WalletUI>().AddMoney(-heldItem.price);

            // change stock of held item in memory
            int shopIndex = gameManager.shopMemory.FindIndex(shopData => shopData.name == currentShop.name);
            int itemIndex = gameManager.shopMemory[shopIndex].stock.FindIndex(item => item.name == heldItem.name);
            gameManager.shopMemory[shopIndex].stock[itemIndex] = newHeldItem;

            // change the local stock of this item
            dialogueManager.RefreshShop(currentShop);

            // start purchasing dialogue
            dialogueManager.currentDialogueStateMachine.SetTrigger("Purchase");
        }
    }

    public void SetAsSelectedItem()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
