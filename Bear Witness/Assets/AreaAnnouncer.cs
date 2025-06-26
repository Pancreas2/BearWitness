using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreaAnnouncer : MonoBehaviour
{
    List<LevelLoader.LevelArea> visited = new();
    public TextMeshProUGUI areaText1;
    public TextMeshProUGUI areaText2;
    public Animator animator;

    private void OnLevelWasLoaded(int level)
    {
        LevelLoader loader = FindObjectOfType<LevelLoader>();
        LevelLoader.LevelArea area = loader.area;
        if (!visited.Contains(area) && !loader.hideAreaFlag)
        {
            if (area != LevelLoader.LevelArea.Sigilroom)
            {
                (string, string) names = AreaToStrings(area);
                areaText1.text = names.Item1;
                areaText2.text = names.Item2;
                animator.SetTrigger("Start");
            }

            visited.Add(area);

        }
    }

    private (string, string) AreaToStrings(LevelLoader.LevelArea levelArea)
    {
        switch(levelArea)
        {
            case LevelLoader.LevelArea.Hollow:
                return ("Overgrown", "Hollow");
            case LevelLoader.LevelArea.ShoresVillage:
                return ("Silverstone", "Wharf");
            case LevelLoader.LevelArea.Airship:
                return ("The Tumbleweed", "");
            case LevelLoader.LevelArea.Twixt:
                return ("???", "");
            default:
                return (levelArea.ToString(), "");
        }
    }
}
