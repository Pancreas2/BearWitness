using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class BenchMenu : MonoBehaviour
{
    public List<ToolSlot> tools;
    public List<ToolSlot> items;
    public bool menuOpen;

    [SerializeField] Animator animator;

    [SerializeField] GameObject defaultSelect;

    private List<int> equippedSlots = new();

    GameObject lastSelectedSlot;

    public List<Image> toolImages;
    [SerializeField] Sprite defaultToolImage;

    [Header("Wait menu")]

    [SerializeField] Slider totalTimeSlider;
    [SerializeField] Slider addedTimeSlider;
    [SerializeField] TextMeshProUGUI timeOutput;

    [Header("Save menu")]

    [SerializeField] GameObject timeline;
    [SerializeField] GameObject node;
    [SerializeField] GameObject placer;

    [Header("Badge menu")]

    [SerializeField] BadgePlacer badgePlacer;

    [System.Serializable]
    private enum ActiveMenu {
        Tool,
        Wait,
        Badge,
        None
    }

    private ActiveMenu currentMenu = ActiveMenu.None;

    private float waitTime = 0f;
    private float currentTime = 0f;
    private float totalCapacity = 600f;
    private float mult = 1f;
    private float maxWaitTime = 500f;

    [Header("Buttons")]

    [SerializeField] Button toolBtn;
    [SerializeField] Button waitBtn;
    [SerializeField] Button badgeBtn;

    [Header("Icons")]

    [SerializeField] Sprite benchIcon;
    [SerializeField] Sprite shatterIcon;
    [SerializeField] Sprite fragmentIcon;
    [SerializeField] Sprite damageIcon;

    [Header("Tabs")]

    [SerializeField] GameObject toolTab;
    [SerializeField] GameObject waitTab;
    [SerializeField] GameObject badgeTab;

    [Header("SelectionPriority")]
    [SerializeField] GameObject mainMenuDefault;
    [SerializeField] GameObject toolMenuDefault;
    [SerializeField] GameObject waitMenuDefault;
    [SerializeField] GameObject badgeMenuDefault;

    public void Load()
    {
        for (int i = 0; i < tools.Count; i++)
        {
            InventorySlot slot = tools[i].GetComponent<InventorySlot>();
            if (i < GameManager.instance.tools.Count)
            {
                slot.heldItem = Resources.Load<Item>(GameManager.instance.tools[i]);
            } else
            {
                slot.heldItem = Resources.Load<Item>("Null Item");
            }

            slot.FindItem();
            slot.ReloadImage();
        }

        for (int i = 0; i < items.Count; i++)
        {
            InventorySlot slot = items[i].GetComponent<InventorySlot>();
            if (i < GameManager.instance.items.Count)
            {
                slot.heldItem = Resources.Load<Item>(GameManager.instance.items[i].item);
            }
            else
            {
                slot.heldItem = Resources.Load<Item>("Null Item");
            }

            slot.FindItem();
            slot.ReloadImage();
            items[i].Reload();  // makes stack amount visible
        }

        waitBtn.interactable = !(GameManager.instance.panicMode || GameManager.instance.inArktis);

        currentTime = GameManager.instance.hourglassFill;
        totalCapacity = GameManager.instance.hourglassCapacity;
        totalTimeSlider.value = currentTime / totalCapacity;

        mult = GameManager.instance.timeMultiplier;
        maxWaitTime = currentTime * mult - GameManager.panicTime;

        addedTimeSlider.value = waitTime / maxWaitTime;
        timeOutput.text = TimeToText(waitTime);

        PopulateTimeline();
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
            if (currentMenu == ActiveMenu.Tool)
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

            if (Input.GetButtonDown("Pause"))
            {
                FindObjectOfType<PauseMenu>().DenyPause(0.25f);  // this way, you don't immediately pause the game when exiting a bench
                SwapTab(ActiveMenu.None);
                SetOpen(false);
            }
        }
    }

    private void ReloadNewSelectionSlots()
    {
        for (int i = 0; i < 3; i++)
        {
            if (GameManager.instance.currentItems[i] == "")
            {
                toolImages[i].sprite = Resources.Load<Item>("Null").image;
            } else
            {
                toolImages[i].sprite = Resources.Load<Item>(GameManager.instance.currentItems[i]).image;
            }
        }
    }

    private void SwapTab(ActiveMenu menuType)
    {
        switch (menuType)
        {
            case ActiveMenu.Tool:
                toolTab.SetActive(true);
                badgeTab.SetActive(false);
                waitTab.SetActive(false);
                EventSystem.current.SetSelectedGameObject(toolMenuDefault);
                break;
            case ActiveMenu.Wait:
                toolTab.SetActive(false);
                badgeTab.SetActive(false);
                waitTab.SetActive(true);
                EventSystem.current.SetSelectedGameObject(waitMenuDefault);
                break;
            case ActiveMenu.Badge:
                toolTab.SetActive(false);
                badgeTab.SetActive(true);
                waitTab.SetActive(false);
                EventSystem.current.SetSelectedGameObject(badgeMenuDefault);
                break;
            case ActiveMenu.None:
                toolTab.SetActive(false);
                badgeTab.SetActive(false);
                waitTab.SetActive(false);
                EventSystem.current.SetSelectedGameObject(mainMenuDefault);
                break;
        }

        if (currentMenu == ActiveMenu.Wait) ApplyWaitTime();
        if (currentMenu == ActiveMenu.Badge) badgePlacer.SetBadgeConfiguration();

        currentMenu = menuType;
    }

    public void SwapTabFromString(string input)
    {
        ActiveMenu targetMenu = ActiveMenu.None;
        switch (input)
        {
            case "Tool":
                targetMenu = ActiveMenu.Tool;
                break;
            case "Wait":
                targetMenu = ActiveMenu.Wait;
                break;
            case "Badge":
                targetMenu = ActiveMenu.Badge;
                break;
        }
        SwapTab(targetMenu);
    }

    private void SetSlotEquipped(int slot)
    {
        FindObjectOfType<MouseSlayer>().IgnoreClick();

        EventSystem.current.currentSelectedGameObject.TryGetComponent<ToolSlot>(out ToolSlot selectedToolSlot);

        if (selectedToolSlot)
        {
            InventorySlot selectedSlot = selectedToolSlot.GetComponent<InventorySlot>();

            // Tool slots are indexed 0-11, item slots 12-23

            int slotInSelection = equippedSlots[slot];
            bool isTool = slotInSelection < 12;
            if (!isTool) slotInSelection -= 12;

            if (slotInSelection > -1)
            {

                if (isTool)
                {
                    tools[slotInSelection].GetComponent<ToolSlot>().UnequipItem();

                    if (tools[slotInSelection] != selectedSlot)
                    {
                        selectedToolSlot.EquipItem(slot);

                        for (int i = 0; i < 3; i++)
                        {
                            if (equippedSlots[i] == selectedSlot.index) equippedSlots[i] = -1;
                        }

                        equippedSlots[slot] = selectedSlot.index;

                        ReloadNewSelectionSlots();
                    }
                } else
                {
                    items[slotInSelection].GetComponent<ToolSlot>().UnequipItem();

                    if (items[slotInSelection] != selectedSlot)
                    {
                        selectedToolSlot.EquipItem(slot);

                        for (int i = 0; i < 3; i++)
                        {
                            if (equippedSlots[i] == selectedSlot.index) equippedSlots[i] = -1;
                        }

                        equippedSlots[slot] = selectedSlot.index;

                        ReloadNewSelectionSlots();
                    }
                }

            }
            else
            {
                selectedToolSlot.EquipItem(slot);
                equippedSlots[slot] = selectedSlot.index;

                ReloadNewSelectionSlots();
            }

            selectedSlot.ReloadImage();
            selectedToolSlot.Reload();
        }
    }

    public void SetOpen(bool open)
    {
        menuOpen = open;
        if (open)
        {
            Load();
            ReloadNewSelectionSlots();
            animator.SetBool("open", true);
            EventSystem.current.SetSelectedGameObject(defaultSelect);
            GameUI_Controller.instance.Reload();
            PauseMenu.GameIsPaused = true;
        }
        else
        {
            PauseMenu.GameIsPaused = false;
            animator.SetBool("open", false);
            BenchInteractable bench = FindObjectOfType<BenchInteractable>();
            GameUI_Controller.instance.Reload();
            EventSystem.current.SetSelectedGameObject(null);
            if (bench) bench.Exit();
        }
    }

    public void AddWaitTime(float value)
    {
        waitTime += value;

        Debug.Log("Adding " + value + " seconds");

        if (waitTime < 0) waitTime = 0;

        if (currentTime - (waitTime + GameManager.panicTime) / mult <= 0)
        {
            waitTime = maxWaitTime;
        }

        addedTimeSlider.value = waitTime / maxWaitTime;
        timeOutput.text = TimeToText(waitTime);
    }

    private string TimeToText(float time)
    {
        string output = "";
        if (time >= 60)
        {
            output += Mathf.Floor(time / 60f) + " M ";
            time %= 60f;
        }

        output += Mathf.Floor(time) + " S";

        return output;
    }

    public void ApplyWaitTime()
    {
        GameManager.instance.hourglassFill -= waitTime / mult;
        GameManager.instance.gameTime += waitTime;
        waitTime = 0;
        addedTimeSlider.value = 0;
        timeOutput.text = TimeToText(0);
        Load();
    }

    public void SaveSegment()
    {
        GameManager.instance.SaveCurrentSegment(SceneManager.GetActiveScene().name);
    }

    public void PopulateTimeline()
    {
        foreach (TimelineNode node in FindObjectsByType<TimelineNode>(FindObjectsSortMode.None))
        {
            Destroy(node.gameObject);
        }

        GameManager.instance.PrepSegmentForSave(SceneManager.GetActiveScene().name);

        RunSegment segment = GameManager.instance.GetSegment();
        float midpointTime = segment.startTime + 0.5f * segment.elapsedTime;
        float stretchFactor = 400f / segment.elapsedTime;

        if (stretchFactor == float.NaN) return;  // for the case when only one event (and thus 0 time) is registered

        foreach (float time in segment.timesOfInterest.Keys)
        {
            placer.transform.localPosition = new(stretchFactor * (time - midpointTime), 0f, 0f);

            TimelineNode newNode = Instantiate(node, parent: placer.transform).GetComponent<TimelineNode>();

            newNode.transform.SetParent(timeline.transform);

            switch (segment.timesOfInterest[time])
            {
                case "Damage":
                    newNode.icon.sprite = damageIcon;
                    break;
                case "Shatter":
                    newNode.icon.sprite = shatterIcon;
                    break;
                case "Fragment":
                    newNode.icon.sprite = fragmentIcon;
                    break;
                default:
                    newNode.icon.sprite = benchIcon;
                    break;
            }
        }
    }
}
