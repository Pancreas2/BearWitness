using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    private bool invOpen = false;
    public GameObject resumeMenuUI;
    public InventorySlot[] toolSlots = new InventorySlot[12];
    public InventorySlot[] itemSlots = new InventorySlot[24];
    private GameManager gameManager;
    public InventorySlot lastSelectedSlot = new();

    private List<int> equippedSlots = new();

    private void Start()
    {
        equippedSlots.Add(-1);
        equippedSlots.Add(-1);
        equippedSlots.Add(-1);
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
        if (Input.GetButtonDown("Inventory"))
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

        // if it's stupid and it works, it isn't stupid.
        if (invOpen)
        {
            if (Input.GetButtonDown("AttackX"))
            {
                SetSlotEquipped(0);
            } else if (Input.GetButtonDown("AttackY"))
            {
                SetSlotEquipped(1);
            } else if (Input.GetButtonDown("AttackB"))
            {
                SetSlotEquipped(2);
            }
        }
    }

    private void SetSlotEquipped(int slot)
    {
        EventSystem.current.currentSelectedGameObject.TryGetComponent<ToolSlot>(out ToolSlot selectedToolSlot);
        if (selectedToolSlot)
        {
            InventorySlot selectedSlot = selectedToolSlot.GetComponent<InventorySlot>();

            if (equippedSlots[slot] > -1)
            {
                toolSlots[equippedSlots[slot]].GetComponent<ToolSlot>().UnequipItem();
                if (toolSlots[equippedSlots[slot]] != selectedSlot)
                {
                    selectedToolSlot.EquipItem(slot);

                    for (int i = 0; i < 3; i++)
                    {
                        if (equippedSlots[i] == selectedSlot.index) equippedSlots[i] = -1;
                    }
                    
                    equippedSlots[slot] = selectedSlot.index;
                }
            }
            else
            {
                selectedToolSlot.EquipItem(slot);
                equippedSlots[slot] = selectedSlot.index;
            }
        }
    }

    public void Resume()
    {
        invOpen = false;
        resumeMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        resumeMenuUI.GetComponentInParent<GameUI_Controller>().Reload();
    }

    public void Pause()
    {
        invOpen = true;
        if (PauseMenu.GameIsPaused) FindObjectOfType<PauseMenu>().Resume();
        resumeMenuUI.SetActive(true);
        foreach (InventorySlot slot in toolSlots)
        {
            if (slot)
            {
                slot.FindItem();
                slot.ReloadImage();
            }
        }
        foreach (InventorySlot slot in itemSlots)
        {
            if (slot)
            {
                slot.FindItem();
                slot.ReloadImage();
            }
        }
        Time.timeScale = 0f;
        GameIsPaused = true;

        EventSystem.current.SetSelectedGameObject(toolSlots[0].gameObject);
    }

    public void SwapInventoryPosition(int a, int b, bool tool)
    {
        if (tool)
        {
            Item itemHolder = gameManager.tools[a];
            gameManager.tools[a] = gameManager.tools[b];
            gameManager.tools[b] = itemHolder;

            for (int i = 0; i < 3; i++)
            {
                if (a == i || b == i)
                {
                    if (gameManager.tools[i] != null)
                    {
                        gameManager.currentItems[i] = gameManager.tools[i];
                    }
                    else
                    {
                        gameManager.currentItems[i] = new();
                    }
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