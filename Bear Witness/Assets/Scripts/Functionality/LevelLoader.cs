using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0.5f;
    public string levelMusic;
    [SerializeField] private bool doStartCutscene = true;

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
        player.frozen = true;
        yield return new WaitForSeconds(transitionTime);
        GameManager.instance.pauseGameTime = false;
        player.frozen = false;
    }
}