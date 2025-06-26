using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    TitleScreenTheme.TitleTheme currentTheme = TitleScreenTheme.TitleTheme.Arktis;
    public Image titleBackdrop;
    public Image title;
    public Image backgroundColor;

    [SerializeField] private TextMeshProUGUI themeOutput;
    [SerializeField] private TextMeshProUGUI musicOutput;
    [SerializeField] private TextMeshProUGUI soundOutput;

    private bool updateVolume = false;

    void Awake()
    {
        if (PlayerPrefs.HasKey("theme"))
        {
            currentTheme = Resources.Load<TitleScreenTheme>(PlayerPrefs.GetString("theme")).theme;
        } else
        {
            PlayerPrefs.SetString("theme", "Arktis");
        }

        RefreshTheme();
        soundOutput.text = Mathf.Round(PlayerPrefs.GetFloat("sound") * 10).ToString();
        musicOutput.text = Mathf.Round(PlayerPrefs.GetFloat("music") * 10).ToString();
        themeOutput.text = PlayerPrefs.GetString("theme");
    }

    public void AdvanceTheme()
    {
        List<TitleScreenTheme.TitleTheme> themes = GameManager.instance.unlockedThemes;
        int index = themes.IndexOf(currentTheme);
        if (index + 1 == themes.Count)
            currentTheme = themes[0];
        else currentTheme = themes[index + 1];
        PlayerPrefs.SetString("theme", currentTheme.ToString());
        RefreshTheme();
    }

    public void RetractTheme()
    {
        List<TitleScreenTheme.TitleTheme> themes = GameManager.instance.unlockedThemes;
        int index = themes.IndexOf(currentTheme);
        if (index == 0)
            currentTheme = themes[themes.Count - 1];
        else currentTheme = themes[index - 1];
        PlayerPrefs.SetString("theme", currentTheme.ToString());
        RefreshTheme();
    }

    public void RefreshTheme()
    {
        TitleScreenTheme theme = Resources.Load<TitleScreenTheme>(currentTheme.ToString());
        titleBackdrop.sprite = theme.titleScreenImage;
        title.sprite = theme.titleVariant;
        backgroundColor.color = theme.backgroundColor;
        AudioManager.instance.StopAll(1);
        AudioManager.instance.Play(theme.songTitle, 0, 1, 1);
        if (themeOutput) themeOutput.text = theme.name;
    }

    public void ChangeMusicFactor(float change)
    {
        float currentFactor = AudioManager.instance.musicVolFactor;
        if (currentFactor + change < 0)
        {
            AudioManager.instance.musicVolFactor = 0;
            musicOutput.text = "0";
            PlayerPrefs.SetFloat("music", 0);
        } else if (currentFactor + change > 1)
        {
            AudioManager.instance.musicVolFactor = 1;
            musicOutput.text = "10";
            PlayerPrefs.SetFloat("music", 1);
        } else
        {
            AudioManager.instance.musicVolFactor = currentFactor + change;
            musicOutput.text = Mathf.Round((currentFactor + change) * 10).ToString();
            PlayerPrefs.SetFloat("music", currentFactor + change);
        }

        updateVolume = true;
    }

    public void ChangeSoundFactor(float change)
    {
        float currentFactor = AudioManager.instance.soundVolFactor;
        if (currentFactor + change < 0)
        {
            AudioManager.instance.soundVolFactor = 0;
            soundOutput.text = "0";
            PlayerPrefs.SetFloat("sound", 0);
        }
        else if (currentFactor + change > 1)
        {
            AudioManager.instance.soundVolFactor = 1;
            soundOutput.text = "10";
            PlayerPrefs.SetFloat("sound", 1);
        }
        else
        {
            AudioManager.instance.soundVolFactor = currentFactor + change;
            soundOutput.text = Mathf.Round((currentFactor + change) * 10).ToString();
            PlayerPrefs.SetFloat("sound", currentFactor + change);
        }

        updateVolume = true;
    }

    public void SaveChanges()
    {
        Debug.Log("Saved");
        PlayerPrefs.Save();
    }

    private void LateUpdate()
    {
        if (updateVolume)
        {
            AudioManager.instance.RecalibrateVolume();
            updateVolume = false;
        }
    }
}
