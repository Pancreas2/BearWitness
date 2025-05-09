using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0.5f;
    public string overrideLevelMusic;
    [SerializeField] private bool doStartCutscene = true;
    public enum LevelArea
    {
        Arktis,
        Shores,
        ShoresVillage,
        Lighthouse,
        Hollow,
        Airship,
        Ruins,
        Sigilroom
    }

    public LevelArea area;

    private void Start()
    {
        if (doStartCutscene)
            StartCoroutine(StartScene());
    }

    public void LoadNextLevel(string levelName)
    {
        GameManager.instance.pauseGameTime = true;
        StartCoroutine(LoadLevel(levelName));
    }

    IEnumerator LoadLevel(string levelIndex)
    {
        FindObjectOfType<GameManager>().previousLevel = SceneManager.GetActiveScene().name;
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator StartScene()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player) player.frozen = true;
        yield return new WaitForSeconds(transitionTime);
        GameManager.instance.pauseGameTime = false;
        if (GameUI_Controller.instance) GameUI_Controller.instance.Reload();
        if (player) player.frozen = false;
    }
}
