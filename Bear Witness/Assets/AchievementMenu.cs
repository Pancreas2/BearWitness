using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementMenu : MonoBehaviour
{
    [SerializeField] private List<string> achievements;
    [SerializeField] private List<AchievementDisplay> displays;
    [SerializeField] private TextMeshProUGUI pageText;
    private int pageNum = 1;

    public void Reload()
    {
        for (int i = 0; i < 8; i++)
        {
            int index = (pageNum - 1) * 8 + i;

            if (index < achievements.Count)
            {
                displays[i].ChangeHeldAchievement(Resources.Load<Achievement>("Achievements/" + achievements[index]));
            } else
            {
                displays[i].SetNoAchievement();
            }
        }
    }

    void Start()
    {
        Reload();
    }

    public void ChangePage(int increment)
    {
        int max = Mathf.CeilToInt(achievements.Count / 8f);
        pageNum += increment;
        if (pageNum < 1) pageNum = 1;
        if (pageNum > max) pageNum = max;
        pageText.text = pageNum + " / " + max;
        Reload();
    }
}
