using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BenchMenu : MonoBehaviour
{
    public List<ToolSlot> tools;
    public bool menuOpen;

    [SerializeField] Animator animator;

    [SerializeField] GameObject defaultSelect;

    private List<int> equippedSlots = new();

    GameObject lastSelectedSlot;

    public List<Image> toolImages;
    [SerializeField] Sprite defaultToolImage;

    public void Load()
    {
        for (int i = 0; i < tools.Count; i++)
        {
            InventorySlot slot = tools[i].GetComponent<InventorySlot>();
            slot.heldItem = GameManager.instance.tools[i];
            slot.ReloadImage();
        }
    }

    void Start()
    {
        equippedSlots.Add(-1);
        equippedSlots.Add(-1);
        equippedSlots.Add(-1);
    }

    void Update()
    {
        if (menuOpen)
        {
            if (Input.GetButtonDown("AttackX"))
            {
                SetSlotEquipped(0);
            }
            else if (Input.GetButtonDown("AttackY"))
            {
                SetSlotEquipped(1);
            }
            else if (Input.GetButtonDown("AttackB"))
            {
                SetSlotEquipped(2);
            }
        }
    }

    private void SetSlotEquipped(int slot)
    {
        FindObjectOfType<MouseSlayer>().IgnoreClick();
        EventSystem.current.currentSelectedGameObject.TryGetComponent<ToolSlot>(out ToolSlot selectedToolSlot);

        if (selectedToolSlot)
        {
            InventorySlot selectedSlot = selectedToolSlot.GetComponent<InventorySlot>();

            if (equippedSlots[slot] > -1)
            {
                tools[equippedSlots[slot]].GetComponent<ToolSlot>().UnequipItem();
                if (tools[equippedSlots[slot]] != selectedSlot)
                {
                    selectedToolSlot.EquipItem(slot);
                    toolImages[equippedSlots[slot]].sprite = defaultToolImage;

                    for (int i = 0; i < 3; i++)
                    {
                        if (equippedSlots[i] == selectedSlot.index) equippedSlots[i] = -1;
                    }

                    equippedSlots[slot] = selectedSlot.index;
                    toolImages[slot].sprite = selectedSlot.heldItem.image;
                }
            }
            else
            {
                selectedToolSlot.EquipItem(slot);
                equippedSlots[slot] = selectedSlot.index;
                toolImages[slot].sprite = selectedSlot.heldItem.image;
            }
        }
    }

    public void SetOpen(bool open)
    {
        menuOpen = open;
        if (open)
        {
            animator.SetBool("open", true);
            EventSystem.current.SetSelectedGameObject(defaultSelect);
            PauseMenu.GameIsPaused = true;
        }
        else
        {
            PauseMenu.GameIsPaused = false;
            animator.SetBool("open", false);
            BenchInteractable bench = FindObjectOfType<BenchInteractable>();
            GameUI_Controller.instance.Reload();
            if (bench) bench.Exit();
        }
    }
}
