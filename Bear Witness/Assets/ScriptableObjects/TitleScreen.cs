using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{

    [SerializeField] TitleScreenTheme currentTheme;

    [SerializeField] Image background;
    [SerializeField] Image titleScreen;
    [SerializeField] Image logo;
    [SerializeField] Image[] buttons;


    public void ChangeTheme(TitleScreenTheme newTheme)
    {
        background.CrossFadeColor(newTheme.backgroundColor, 0f, true, false);
        titleScreen.sprite = newTheme.titleScreenImage;
        logo.color = newTheme.titleColor;
        foreach (Image button in buttons)
        {
            button.color = newTheme.buttonColor;
        }

        currentTheme = newTheme;
    }
}
