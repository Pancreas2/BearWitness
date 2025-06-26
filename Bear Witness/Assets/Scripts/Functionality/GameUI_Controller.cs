using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUI_Controller : MonoBehaviour
{
    public static GameUI_Controller instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("DESTROYING GUIC!!");
            Destroy(gameObject);
            gameObject.SetActive(false);
            return;
        }

        GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();

        DontDestroyOnLoad(gameObject);
    }

    private GameManager gameManager;
    [SerializeField] public Hourglass hourglass;

    [SerializeField] private Sprite emptyToolSlot;

    [SerializeField] public List<Image> toolSlots;

    [SerializeField] public List<BadgeSlot> badgeSlots;

    [SerializeField] private GameObject inventoryMenuRoot;

    [SerializeField] private Animator benchMenuAnimator;
    [SerializeField] private GameObject benchMenuDefault;
    public ItemPopup itemPopup;

    [SerializeField] Animator animator;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        ShowAll();
        Reload();

        inventoryMenuRoot.SetActive(false);
    }

    public void Reload()
    {
        for (int i = 0; i < 3; i++)
        {
            if (GameManager.instance.currentItems[i] != null) DisplayHeldItem(Resources.Load<Item>(GameManager.instance.currentItems[i]), i);
        }

        foreach (BadgeSlot badgeSlot in badgeSlots)
        {
            badgeSlot.Reload();
        }

        FindObjectOfType<WalletUI>().ExitShop();  // ensures wallet closes when room refreshes

        GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
    }

    public void DisplayHeldItem(Item item, int slot)
    {
        if (!item || item.image == null) 
        {
            toolSlots[slot].sprite = emptyToolSlot;
        } else
        {
            toolSlots[slot].sprite = item.image;
        }
    }
    public void HideAll()
    {
        animator.SetBool("Hidden", true);
    }

    public void ShowAll()
    {
        animator.SetBool("Hidden", false);
    }
}
