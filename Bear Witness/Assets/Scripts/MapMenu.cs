using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapMenu : MonoBehaviour
{
    [SerializeField] private List<Sprite> maps;
    [SerializeField] private List<LevelLoader.LevelArea> areaReference;
    [SerializeField] private GameObject compassDisplay;
    [SerializeField] private SerializableDictionary<string, Vector2> compassPositionMatcher;
    [SerializeField] private string sceneNameForDictCreation;

    public RawImage targetImage;

    public void LoadMap()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (GameManager.instance.currentBadges.Contains("Compass") && compassPositionMatcher.ContainsKey(sceneName))
        {
            compassDisplay.SetActive(true);
            compassDisplay.transform.localPosition = compassPositionMatcher[sceneName];
        } else
        {
            compassDisplay.SetActive(false);
        }

        //LevelLoader loader = FindObjectOfType<LevelLoader>();
        //int i = areaReference.IndexOf(loader.area);
        //if (i < maps.Count)
        //    targetImage.texture = maps[i].texture;
    }

    public void LogCompassPosition()
    {
        compassPositionMatcher[sceneNameForDictCreation] = compassDisplay.transform.localPosition;
    }
}
