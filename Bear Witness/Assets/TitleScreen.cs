using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{

    [SerializeField] TitleScreenTheme currentTheme;

    [SerializeField] SpriteRenderer backgroundColor;
    [SerializeField] SpriteRenderer titleScreen;
    [SerializeField] Image logo;
    [SerializeField] Image[] buttons;


    public void ChangeTheme(TitleScreenTheme newTheme)
    {
        backgroundColor.color = newTheme.backgroundColor;
        titleScreen.sprite = newTheme.titleScreenImage;
        logo.color = newTheme.titleColor;
        foreach (Image button in buttons)
        {
            button.color = newTheme.buttonColor;
        }

        currentTheme = newTheme;
    }
}
