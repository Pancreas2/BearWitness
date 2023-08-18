using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

    public class GameManager : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
            SceneManager.LoadScene("Arktis_Den");
        }

        private void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        }

        public string previousLevel = "Start";
    }

