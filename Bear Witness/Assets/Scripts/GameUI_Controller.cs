using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private Vector3 hpBasePos = Vector3.up * 150 + Vector3.left * 225;

    public Image toolSlot;

    [SerializeField] private GameObject inventoryMenuRoot;
    public ItemPopup itemPopup;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager.currentItem != null) DisplayHeldItem(gameManager.currentItem);
        


        inventoryMenuRoot.SetActive(false);
    }

    public void Reload()
    {
        if (gameManager.currentItem != null) DisplayHeldItem(gameManager.currentItem);
    }

    public void DisplayHeldItem(Item item)
    {
        if (!item || item.image == null) 
        {
            toolSlot.sprite = emptyToolSlot;
        } else
        {
            toolSlot.sprite = item.image;
        }
    }
}
