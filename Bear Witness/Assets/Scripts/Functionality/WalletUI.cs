using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WalletUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI budgetDisplay;
    [SerializeField] private TextMeshProUGUI modifierDisplay;

    private GameManager gameManager;
    private int modifier;
    private int maxModifier;
    private int budget;
    private float delayTime;

    private void Start()
    {
        gameManager = GameManager.instance;
        modifier = 0;
        budget = gameManager.money;
        budgetDisplay.text = budget.ToString();
    }

    public void AddMoney(int value)
    {
        modifier += value;
        delayTime = Time.time + 1f;
        if (modifier > maxModifier) maxModifier = modifier;
    }

    private void Update()
    {
        if (modifier == 0)
        {
            maxModifier = 0;
            modifierDisplay.text = "";
        }
        else {
            if (Time.time > delayTime)
            {
                int maxDelta = Mathf.Max(Mathf.FloorToInt(maxModifier / 101), 1) * Mathf.RoundToInt(Mathf.Sign(modifier));
                int delta = maxDelta;
                if (Mathf.Abs(modifier) < maxDelta)
                {
                    delta = modifier;
                }

                budget += delta;
                modifier -= delta;

                budgetDisplay.text = budget.ToString();
            }

            if (modifier > 0)
            {
                modifierDisplay.text = "+" + modifier.ToString();
            }
            else
            {
                modifierDisplay.text = modifier.ToString();
            }
        }
    }
}
