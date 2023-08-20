using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

    public class GameManager : MonoBehaviour
    {
        void Awake()
        {
            if (FindObjectsOfType<GameManager>().Length > 1)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(this);
        }

        private void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        }

        public string previousLevel = "Start";
        public int playerMaxHealth = 5;
        public int playerCurrentHealth = 5;
    }

