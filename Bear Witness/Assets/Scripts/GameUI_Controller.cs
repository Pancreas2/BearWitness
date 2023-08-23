using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_Controller : MonoBehaviour
{
    public GameObject hpimg;
    private GameManager gameManager;
    private int maxHealth;
    private int currentHealth;
    private Image[] hpDisplay;
    public Sprite emptyIcon;
    public Sprite fullIcon;
    public Sprite bonusIcon;
    private Vector3 hpBasePos = Vector3.up * 185 + Vector3.left * 275;

    public Image toolSlot;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        maxHealth = gameManager.playerMaxHealth;
        currentHealth = gameManager.playerCurrentHealth;

    }
    void Start()
    {
        if (gameManager.currentItem != null) DisplayHeldItem(gameManager.currentItem);
        hpDisplay = new Image[maxHealth];
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHPImg = Instantiate(hpimg);
            newHPImg.transform.parent = gameObject.transform;
            newHPImg.transform.SetLocalPositionAndRotation((hpBasePos + Vector3.right * i * 50), Quaternion.identity);
            hpDisplay.SetValue(newHPImg.GetComponent<Image>(), i);
        }
    }

    public void DecreaseHP(int damage)
    {
        for (int i = gameManager.playerCurrentHealth; i > gameManager.playerCurrentHealth - damage; i--)
        {
            hpDisplay[i - 1].sprite = emptyIcon;
        }
        gameManager.playerCurrentHealth -= damage;
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
            newHPImg.transform.SetLocalPositionAndRotation((hpBasePos + Vector3.right * (i + maxHealth) * 50), Quaternion.identity);
            hpDisplay.SetValue(newHPImg.GetComponent<Image>(), i);
            hpDisplay[i].sprite = bonusIcon;
        }
    }

    public void DisplayHeldItem(CollectableItem item)
    {
        if (item.icon != null) toolSlot.sprite = item.icon;
    }
}
