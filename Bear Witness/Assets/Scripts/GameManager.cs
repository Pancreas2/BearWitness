using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using System.Dynamic;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
            return;
        }

        DontDestroyOnLoad(gameObject);

        tools = new(12);
        items = new(24);

        for (int i = 0; i < 20; i++)
        {
            doorStates.Add(false);
        }
    }

    private void ReloadLevel()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }

    public void PickupItem(Item item)
    {
        InventoryMenu invManager = FindObjectOfType<InventoryMenu>();

        if (item.type == Item.ItemType.Tool)
        {
            if (!currentItem)
            {
                currentItem = item;
                FindObjectOfType<GameUI_Controller>().DisplayHeldItem(item);
            }

            tools.Add(item);

            // render in menu
            foreach (InventorySlot toolSlot in invManager.toolSlots)
            {
                toolSlot.FindItem();
            }
        } else
        {
            items.Add(item);

            // render in menu
            foreach (InventorySlot itemSlot in invManager.itemSlots)
            {
                itemSlot.FindItem();
            }
        }
    }

    public void ChangeScene(string destination)
    {
        fileTime += Time.timeSinceLevelLoad;
        SceneManager.LoadScene(destination);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public int GetActiveSceneID()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void SavePlayerData(int slot)
    {
        previousLevel = SceneManager.GetActiveScene().name;
        SaveSystem.SavePlayer(this, slot);
    }

    public void LoadPlayerData(int slot)
    {
        string newData = SaveSystem.LoadPlayer(slot);

        // change all the things
        JsonUtility.FromJsonOverwrite(newData, this);
    }

    private void Update()
    {
        if (!pauseGameTime)
            gameTime += Time.deltaTime * 200f / 3f;

        if (gameTime > 10080f)
        {
            Debug.Log("Kablooie");
        }
    }

    public int fileNumber;
    public string fileName;
    public float fileTime;
    public int fileCompletion;

    public bool pauseGameTime = false;
    public float gameTime;

    public string previousLevel;
    public int playerMaxHealth = 5;
    public int playerCurrentHealth = 5;
    public Item currentItem;
    public List<Item> tools;
    public List<Item> items;
    public List<string> playedCutscenes = new();
    public List<string> playedLines = new();
    public List<string> foundItems = new();
    public List<string> permanentFoundItems = new();
    public List<bool> doorStates = new();

    public int money = 30;

    public List<NPCData> npcMemory = new();
}

