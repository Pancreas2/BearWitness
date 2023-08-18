using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class LevelLoader : MonoBehaviour
    {
        public GameObject gameManager;
        public Animator transition;
        public float transitionTime = 0.5f;

        public void LoadNextLevel(string levelName)
        {
            StartCoroutine(LoadLevel(levelName));
        }

        IEnumerator LoadLevel(string levelIndex)
        {
            FindObjectOfType<GameManager>().previousLevel = SceneManager.GetActiveScene().name;
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);

            SceneManager.LoadScene(levelIndex);
        }
    }
