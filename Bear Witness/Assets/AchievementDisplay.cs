using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Image image;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite defaultGoldImage;

    public void ChangeHeldAchievement(Achievement newAch)
    {
        bool hasAchievement = PlayerPrefs.GetInt(newAch.name, 0) == 1;

        if (hasAchievement)
        {
            image.color = Color.white;
            nameText.text = newAch.name;
            descText.text = newAch.description;
            image.sprite = newAch.image;
        } else
        {
            if (newAch.isSecret)
            {
                nameText.text = "Secret Achievement";
                descText.text = "???";
                if (newAch.isGold)
                {
                    image.sprite = defaultGoldImage;
                }
                else
                {
                    image.sprite = defaultImage;
                }
            } else
            {
                nameText.text = newAch.name;
                descText.text = newAch.description;
                image.sprite = newAch.image;
            }

            image.color = Color.gray;
            nameText.color = Color.black;
            descText.color = Color.black;
        }
    }

    public void SetNoAchievement()
    {
        nameText.text = "";
        descText.text = "";
        image.sprite = Resources.Load<Sprite>("null_image");
    }
}
