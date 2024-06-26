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

        for (int i = 0; i < 20; i++)
        {
            uniqueEnemies.Add(false);
        }
    }

    private void Start()
    {
        guic = FindObjectOfType<GameUI_Controller>();

        // temporary!!
        StartRun();
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }

    public void PickupItem(Item item)
    {
        InventoryMenu invManager = FindObjectOfType<InventoryMenu>();

        if (!guic) guic = FindObjectOfType<GameUI_Controller>();
        guic.itemPopup.QueueItem(item);

        if (item.type == Item.ItemType.Tool)
        {
            if (!currentItem)
            {
                currentItem = item;
                guic.DisplayHeldItem(item);
            }

            tools.Add(item);

            // render in menu
            foreach (InventorySlot toolSlot in invManager.toolSlots)
            {
                if (toolSlot)
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

    public void RespawnPlayer()
    {
        currentRespawnPoint.Spawn();
    }

    public void ChangeSpawnPoint(SpawnPlayer newSpawn)
    {
        currentRespawnPoint = newSpawn;
    }

    public void DamagePlayer(float damage)
    {
        hourglassFill -= damage * 10f;
        guic.hourglass.DamageFlash(damage);
    }

    public void HealPlayer(float heal)
    {
        hourglassFill = Mathf.Min(hourglassCapacity, hourglassFill + heal);
    }

    public void StartRun()
    {
        inArktis = false;
    }

    private void Update()
    {
        if (!pauseGameTime)
        {
            gameTime += Time.deltaTime * 2f / 3f;

            if (!inArktis)
            {
                hourglassFill -= Time.deltaTime;
            }
        }

        if (hourglassFill < 0f)
        {
            Debug.Log("Run Ends Here!!");
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

    public float hourglassFill = 300f;
    public float hourglassCapacity = 300f;

    public bool inArktis = true;

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
    public List<ShopData> shopMemory = new();

    private SpawnPlayer currentRespawnPoint;

    public List<bool> uniqueEnemies = new();
    public SlainEnemies slainEnemies;

    private GameUI_Controller guic;
}

