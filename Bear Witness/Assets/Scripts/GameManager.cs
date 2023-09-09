using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using System.Dynamic;

public class GameManager : MonoBehaviour
    {
        void Awake()
        {
        inventory = new CollectableItem[16];
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

    public void PickupItem(CollectableItem item)
    {
        Debug.Log(inventory.Length);
        if (inventoryNextSpace == 0)
        {
            currentItem = item;
            FindObjectOfType<GameUI_Controller>().DisplayHeldItem(item);
        }
        inventory[inventoryNextSpace] = item;
        for (int i = 0; i < inventory.Length - 1; i++)
        {
            if (inventory[i] == null)
            {
                inventoryNextSpace = i;
                return;
            }
            Debug.Log(i + " slot full");
        }
    }

    public void ChangeScene(string destination)
    {
        SceneManager.LoadScene(destination);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public string previousLevel = "Start";
        public int playerMaxHealth = 5;
        public int playerCurrentHealth = 5;
        public CollectableItem currentItem;
        public CollectableItem[] inventory;
    public int inventoryNextSpace = 0;
    public List<string> playedCutscenes = new();
    public List<string> playedLines = new();

    public List<NPCData> npcMemory = new();
}

