using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    private bool pausedByThis = false;

    public GameObject pauseMenuUI;
    [SerializeField] private GameObject resumeBtn;
    private float denyPauseTime = 0f;

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && !FindObjectOfType<DialogueManager>().dialogueRunning)
        {
            if (!GameIsPaused || pausedByThis)
            {
                if (GameIsPaused)
                {
                    Resume();
                    pausedByThis = false;
                }
                else if (denyPauseTime < Time.time)
                {
                    Pause();
                    pausedByThis = true;
                    EventSystem.current.SetSelectedGameObject(resumeBtn);
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        GetComponent<MouseSlayer>().SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        GetComponent<MouseSlayer>().SetActive(false);
    }

    public void LoadMenu()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        // deletes save data! intentional
        GameManager gameManager = GameManager.instance;
        GameUI_Controller guic = GameUI_Controller.instance;
        Destroy(guic.gameObject);
        Destroy(gameManager.gameObject);
        SceneManager.LoadScene("Start");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetLoop()
    {
        Resume();
        GameManager.instance.EndRun();
    }

    public void DenyPause(float delay)
    {
        denyPauseTime = Time.time + delay;
    }
}
