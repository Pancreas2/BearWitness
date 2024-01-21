using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject bar;

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

    private void Start()
    {
        bar.SetActive(false);
    }
}
