using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hourglass : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] Animator leftBulb;
    [SerializeField] Animator rightBulb;

    [SerializeField] Image leftSand;
    [SerializeField] Image rightSand;

    [SerializeField] Color damageFlashColor;
    [SerializeField] Color normalColor;
    [SerializeField] Color arktisColor;

    private float capacity = 100f;
    [SerializeField] private float fill = 100f;

    float damageFlashCooldown = 0f;
    bool isFrozen = false;

    private void Start()
    {
        gameManager = GameManager.instance;

        capacity = gameManager.hourglassCapacity;
        fill = gameManager.hourglassFill;
    }

    private void FixedUpdate()
    {
        fill = gameManager.hourglassFill;
        float fillPercent = fill / capacity;
        leftBulb.SetFloat("Fill", fillPercent);
        rightBulb.SetFloat("Fill", 1 - fillPercent);

        isFrozen = gameManager.pauseGameTime;

        if (isFrozen)
        {
            leftSand.color = Color.white;
            rightSand.color = Color.white;
        } else if (damageFlashCooldown >= Time.time)
        {
            leftSand.color = damageFlashColor;
            rightSand.color = damageFlashColor;
        } else if (gameManager.inArktis)
        {
            leftSand.color = arktisColor;
            rightSand.color = arktisColor;
        } else
        {
            leftSand.color = normalColor;
            rightSand.color = normalColor;
        }
    }

    public void Refresh()
    {
        leftBulb.SetTrigger("Refresh");
        rightBulb.SetTrigger("Refresh");
        capacity = gameManager.hourglassCapacity;
        fill = gameManager.hourglassFill;
    }

    public void DamageFlash(float damage)
    {
        damageFlashCooldown = Time.time + damage / 4f;
    }
}
