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

        for (int i = 0; i < 12; i++)
        {
            tools.Add(nullItem);
        }

        for (int i = 0; i < 24; i++)
        {
            items.Add(nullItem);
        }

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
            if (currentItems.Count < 3)
            {
                guic.DisplayHeldItem(item, currentItems.Count);
                currentItems.Add(item);
            }

            int index = tools.IndexOf(nullItem);
            tools[index] = item;

            // render in menu
            foreach (InventorySlot toolSlot in invManager.toolSlots)
            {
                if (toolSlot)
                    toolSlot.FindItem();
            }
        } else
        {
            int index = items.IndexOf(nullItem);
            items[index] = item;

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
        if (panicMode)
        {
            AudioManager.instance.Stop("Fragmentation", fadeTime: 0f);
            AudioManager.instance.Play("Fragmentation", startTime: Mathf.Min(panicTime, panicTime - hourglassFill), fadeTime: 0.25f, delay: 0.25f);
        }
    }

    public void HealPlayer(float heal)
    {
        hourglassFill = Mathf.Min(hourglassCapacity, hourglassFill + heal);

        if (panicMode && hourglassFill > panicTime)
        {
            panicMode = false;
            AudioManager.instance.Stop("Fragmentation", 3);
            string replaceSong = FindObjectOfType<LevelLoader>().levelMusic;
            AudioManager.instance.Play(replaceSong, fadeTime: 3);
        }
    }

    public void StartRun()
    {
        inArktis = false;
    }

    private void Update()
    {
        if (!pauseGameTime || panicMode)
        {
            gameTime += Time.deltaTime;

            if (!inArktis)
            {
                hourglassFill -= Time.deltaTime;
            }
        }

        if (hourglassFill < panicTime && !panicMode)
        {
            panicMode = true;
            AudioManager.instance.StopAll(3);
            AudioManager.instance.Play("Fragmentation", fadeTime: 4);
        }

        if (hourglassFill < 0f)
        {
            Debug.Log("Run Ends Here!!");

            for (int i = 0; i < npcMemory.Count; i++)
            {
                npcMemory[i].spokenTo = false;
            }

            panicMode = false;
            inArktis = true;
            hourglassFill = hourglassCapacity;
            loopNumber++;
            ChangeScene("Arktis_Save_Room");
        }
    }

    public int fileNumber;
    public string fileName;
    public float fileTime;
    public int fileCompletion;
    public int loopNumber = 0;

    public bool pauseGameTime = false;
    public float gameTime;

    public string previousLevel;
    public int playerMaxHealth = 5;
    public int playerCurrentHealth = 5;

    public float hourglassFill = 900f;
    public float hourglassCapacity = 900f;
    private bool panicMode = false;

    public const float panicTime = 76.8f;

    public bool inArktis = true;

    public List<Item> currentItems;
    public List<Item> tools;
    public List<Item> items;
    [SerializeField] private Item nullItem;

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

