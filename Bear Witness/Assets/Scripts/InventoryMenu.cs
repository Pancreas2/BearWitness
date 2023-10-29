using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject resumeMenuUI;
    public InventorySlot[] toolSlots = new InventorySlot[12];
    public InventorySlot[] itemSlots = new InventorySlot[24];
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
    }

    public void Pause()
    {
        resumeMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        foreach (InventorySlot slot in toolSlots)
        {
            if (slot)
            {
                slot.heldItem = gameManager.tools[slot.index];
                slot.ReloadImage();
            }
        }
        foreach (InventorySlot slot in itemSlots)
        {
            if (slot)
            {
                slot.heldItem = gameManager.items[slot.index];
                slot.ReloadImage();
            }
        }
    }

    public void SwapInventoryPosition(int a, int b, bool tool)
    {
        if (tool)
        {
            Item itemHolder = gameManager.tools[a];
            gameManager.tools[a] = gameManager.tools[b];
            gameManager.tools[b] = itemHolder;
            if (a == 0 || b == 0)
            {
                if (gameManager.tools[0] != null)
                {
                    gameManager.currentItem = gameManager.tools[0];
                }
                else
                {
                    gameManager.currentItem = new();
                }
            }
        } else
        {
            Item itemHolder = gameManager.items[a];
            gameManager.items[a] = gameManager.items[b];
            gameManager.items[b] = itemHolder;
        }
    }
}
