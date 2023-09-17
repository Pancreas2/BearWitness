using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject resumeMenuUI;
    public InventorySlot[] inventorySlots = new InventorySlot[9];
    private GameManager gameManager;
    public InventorySlot lastSelectedSlot = new();

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetActiveSlot(InventorySlot slot) {
        if (lastSelectedSlot == slot)
        {
            // force deselect
            EventSystem.current.SetSelectedGameObject(null);
            lastSelectedSlot = new();
        } else
        {
            lastSelectedSlot = slot;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        resumeMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        resumeMenuUI.GetComponentInParent<GameUI_Controller>().DisplayHeldItem(gameManager.currentItem);
        gameManager.FindNextOpenInventorySlot();
    }

    public void Pause()
    {
        resumeMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot)
            {
                slot.heldItem = gameManager.inventory[slot.index];
                slot.ReloadImage();
            }
        }
    }

    public void SwapInventoryPosition(int a, int b)
    {
        CollectableItem itemHolder = gameManager.inventory[a];
        gameManager.inventory[a] = gameManager.inventory[b];
        gameManager.inventory[b] = itemHolder;
        if (a == 0 || b == 0)
        {
            if (gameManager.inventory[0] != null)
            {
                gameManager.currentItem = gameManager.inventory[0];
            } else
            {
                gameManager.currentItem = new();
            }
        }
    }


}
