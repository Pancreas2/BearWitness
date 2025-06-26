using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject bar;
    public Image portrait;

    public void SetHPValue(int health)
    {
        slider.value = health;
    }

    public void SetMaxHP(int health)
    {
        slider.maxValue = health;
    }

    public void SetVisibility(bool active)
    {
        bar.SetActive(active);
    }

    public void SetPortrait(Sprite image)
    {
        if (image)
            portrait.sprite = image;
        else
            Debug.LogWarning("No boss portrait found");
    }

    private void Start()
    {
        bar.SetActive(false);
    }

    private void OnLevelWasLoaded(int level)
    {
        bar.SetActive(false);
    }
}
