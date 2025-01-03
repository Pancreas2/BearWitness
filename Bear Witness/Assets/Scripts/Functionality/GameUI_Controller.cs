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
        else
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private GameManager gameManager;
    [SerializeField] public Hourglass hourglass;

    [SerializeField] private Sprite emptyToolSlot;

    [SerializeField] public List<Image> toolSlots;

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
            if (GameManager.instance.currentItems[i] != null) DisplayHeldItem(GameManager.instance.currentItems[i], i);
        }
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
