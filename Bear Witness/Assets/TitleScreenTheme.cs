using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Title Theme", menuName = "Title Screen Theme")]
[System.Serializable]
public class TitleScreenTheme : ScriptableObject
{
    public enum TitleTheme
    {
        Arktis,
        Shores
    }

    public TitleTheme theme;
    public Color backgroundColor;
    public Sprite titleScreenImage;
    public Color titleColor;
    public Color buttonColor;
    public string songTitle;
}
