using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMenu : MonoBehaviour
{
    [SerializeField] private List<Sprite> maps;
    [SerializeField] private List<LevelLoader.LevelArea> areaReference;

    public RawImage targetImage;

    public void LoadMap()
    {
        LevelLoader loader = FindObjectOfType<LevelLoader>();
        int i = areaReference.IndexOf(loader.area);
        if (i < maps.Count)
            targetImage.texture = maps[i].texture;
    }
}
