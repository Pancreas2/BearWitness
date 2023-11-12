using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_Controller : MonoBehaviour
{
    public GameObject hpimg;
    [SerializeField] private GameObject hpBar;
    private GameManager gameManager;
    private int maxHealth;
    private int currentHealth;
    private Image[] hpDisplay;
    public Sprite emptyIcon;
    public Sprite fullIcon;
    public Sprite bonusIcon;
    [SerializeField] private Sprite emptyToolSlot;
    private Vector3 hpBasePos = Vector3.up * 150 + Vector3.left * 225;

    public Image toolSlot;

    [SerializeField] private GameObject inventoryMenuRoot;
    [SerializeField] private GameObject giveMenuRoot;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        maxHealth = gameManager.playerMaxHealth;
        currentHealth = gameManager.playerCurrentHealth;
        if (gameManager.currentItem != null) DisplayHeldItem(gameManager.currentItem);
        hpDisplay = new Image[maxHealth];
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHPImg = Instantiate(hpimg, hpBar.transform);
            newHPImg.transform.SetLocalPositionAndRotation((hpBasePos + Vector3.right * i * 30f), Quaternion.identity);
            if (currentHealth <= i)
            {
                newHPImg.GetComponent<Image>().sprite = emptyIcon;
            }
            hpDisplay.SetValue(newHPImg.GetComponent<Image>(), i);
        }

        inventoryMenuRoot.SetActive(false);
        giveMenuRoot.SetActive(false);
    }

    public void DecreaseHP(int damage)
    {
        if (gameManager.playerCurrentHealth - damage < 0)
        {
            for (int i = gameManager.playerCurrentHealth; i > 0; i--)
            {
                hpDisplay[i - 1].sprite = emptyIcon;
            }
            gameManager.playerCurrentHealth = 0;
        } else
        {
            for (int i = gameManager.playerCurrentHealth; i > gameManager.playerCurrentHealth - damage; i--)
            {
                hpDisplay[i - 1].sprite = emptyIcon;
            }
            gameManager.playerCurrentHealth -= damage;
        }
    }

    public void IncreaseHP(int heal)
    {
        for (int i = currentHealth; i < Mathf.Min(currentHealth + heal, maxHealth); i--)
        {
            hpDisplay[i].sprite = fullIcon;
        }
        gameManager.playerCurrentHealth += heal;
        gameManager.playerCurrentHealth = Mathf.Min(gameManager.playerCurrentHealth, gameManager.playerMaxHealth);
    }

    public void AddBonusHP(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject newHPImg = Instantiate(hpimg);
            newHPImg.transform.parent = gameObject.transform;
            newHPImg.transform.SetLocalPositionAndRotation((hpBasePos + Vector3.right * (i + maxHealth) * 30f), Quaternion.identity);
            hpDisplay.SetValue(newHPImg.GetComponent<Image>(), i);
            hpDisplay[i].sprite = bonusIcon;
        }
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
