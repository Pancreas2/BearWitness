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
    bool wasFrozen = false;

    bool fadeToNormalColor = false;
    float redChangeRate = 0f;
    float greenChangeRate = 0f;
    float blueChangeRate = 0f;

    Vector3 defaultTransform;

    private void Start()
    {
        gameManager = GameManager.instance;
        defaultTransform = transform.position;
        Refresh();
    }

    private void FixedUpdate()
    {
        fill = gameManager.hourglassFill;
        float fillPercent = fill / capacity;
        leftBulb.SetFloat("Fill", fillPercent);
        rightBulb.SetFloat("Fill", 0.8f - fillPercent);

        isFrozen = gameManager.pauseGameTime;

        if (isFrozen)
        {
            wasFrozen = true;
            leftBulb.speed = 0;
            rightBulb.speed = 0;
            leftSand.color = Color.white;
            rightSand.color = Color.white;
        } else if (damageFlashCooldown >= Time.time)
        {
            leftSand.color = damageFlashColor;
            rightSand.color = damageFlashColor;

            // jitter around
            float randomKick = Mathf.Round(Random.Range(-2f, 2f)) * 2.4f;
            if (Random.value > 0.5f)
            {
                transform.position = new Vector3(transform.position.x + randomKick, transform.position.y);
            } else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + randomKick);
            }

        } else if (gameManager.inArktis)
        {
            leftSand.color = arktisColor;
            rightSand.color = arktisColor;
        } else if (fadeToNormalColor)
        {
            Color newColor = leftSand.color + new Color(redChangeRate, greenChangeRate, blueChangeRate);
            newColor.a = 1f;
            if ((newColor.r > normalColor.r ^ redChangeRate < 0) || newColor.r > 1f)
            {
                redChangeRate = 0f;
                newColor.r = normalColor.r;
            }
            if ((newColor.g > normalColor.g ^ greenChangeRate < 0) || newColor.g > 1f)
            {
                greenChangeRate = 0f;
                newColor.g = normalColor.g;
            }
            if ((newColor.b > normalColor.b ^ blueChangeRate < 0) || newColor.b > 1f)
            {
                blueChangeRate = 0f;
                newColor.b = normalColor.b;
            }

            leftSand.color = newColor;
            rightSand.color = newColor;

            if (newColor == normalColor) fadeToNormalColor = false;

        } else if (leftSand.color != normalColor && damageFlashCooldown < Time.time)
        {
            fadeToNormalColor = true;
            redChangeRate = (normalColor.r - leftSand.color.r) / 20f;
            greenChangeRate = (normalColor.g - leftSand.color.g) / 20f;
            blueChangeRate = (normalColor.b - leftSand.color.b) / 20f;
        } else {
            leftSand.color = normalColor;
            rightSand.color = normalColor;
        }

        if (wasFrozen && !isFrozen)
        {
            leftBulb.speed = 1;
            rightBulb.speed = 1;
            wasFrozen = false;
        }

        if (damageFlashCooldown < Time.time)
        {
            transform.position = defaultTransform;
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
