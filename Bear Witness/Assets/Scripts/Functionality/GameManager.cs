using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using System.Dynamic;
using System;
using Unity.VisualScripting;
using Ink.Runtime;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public readonly static List<Gate.Gates> keepGatesOnLoop = new List<Gate.Gates> { 
        Gate.Gates.CircleSigil 
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
            return;
        }

        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < 12; i++)
        {
            tools.Add("");
        }

        for (int i = 0; i < 24; i++)
        {
            items.Add("");
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

        if (!guic) guic = FindObjectOfType<GameUI_Controller>();
        guic.itemPopup.QueueItem(item);

        if (item.type == Item.ItemType.CircleBadge || item.type == Item.ItemType.SquareBadge || item.type == Item.ItemType.TriangleBadge)
        {
            foundBadges.Add(item.name);
            return;
        }

        InventoryMenu invManager = FindObjectOfType<InventoryMenu>();

        if (item.type == Item.ItemType.Tool)
        {

            int index = tools.IndexOf("");
            tools[index] = item.name;

            // render in menu
            //foreach (InventorySlot toolSlot in invManager.toolSlots)
            //{
            //    if (toolSlot)
            //        toolSlot.FindItem();
            //}
        } else
        {
            int index = items.IndexOf("");
            items[index] = item.name;

            // render in menu
            //foreach (InventorySlot itemSlot in invManager.itemSlots)
            //{
            //    itemSlot.FindItem();
            //}
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
        if (!guic) guic = FindObjectOfType<GameUI_Controller>();
        playerCurrentHealth -= damage;
        guic.hourglass.DamageFlash(damage);

        if (playerCurrentHealth <= 0)
        {
            guic.hourglass.SetBroken(true);

            if (panicMode)
            {
                EndRun();
            }

            timeMultiplier /= 1.5f;
        }
    }

    public void HealPlayer(float heal)
    {
        hourglassFill = Mathf.Min(hourglassCapacity, hourglassFill + heal);

        if (panicMode && hourglassFill > panicTime / timeMultiplier)
        {
            panicMode = false;
            AudioManager.instance.ReloadMusic();
        }
    }

    public void StartRun()
    {
        if (!guic) guic = GameUI_Controller.instance;

        if (playedCutscenes.Contains("enter_shores"))
        {
            playedCutscenes.Remove("enter_shores");
        }

        timeMultiplier = 1f;
        inArktis = false;
        for (int i = 0; i < doorStates.Count; i++)
        {
            bool kept = false;
            foreach (Gate.Gates gate in keepGatesOnLoop)
            {
                if (i == Gate.GateMatch[gate])
                {
                    kept = true;
                }
            }

            if (!kept)
            {
                doorStates[i] = false;
            }
        }

        for (int i = 0; i < 20; i++)
        {
            uniqueEnemies[i] = false;
        }

        guic.hourglass.Refresh();
    }

    public void EndRun()
    {
        loopNumber++;

        SerializableDictionary<string, string> keptStories = new();

        foreach (string story in currentStories.Keys)
        {
            if (keepDialogueOnLoop.Contains(story)) {
                keptStories.Add(story, currentStories[story]);
            }
        }

        currentStories = keptStories;

        for (int i = 0; i < npcMemory.Count; i++)
        {
            npcMemory[i].met = false;
        }

        panicMode = false;
        fileTime += gameTime;
        gameTime = 0f;
        inArktis = true;
        hourglassFill = hourglassCapacity;

        AudioManager.instance.StopAll();

        playerCurrentHealth = playerMaxHealth;
        guic.hourglass.SetBroken(false);

        ChangeScene("The_Twixt");

        PlayerController player = FindObjectOfType<PlayerController>();
        if (tools.Contains("Broken Lantern"))
        {
            int index = tools.IndexOf("Broken Lantern");
            tools[index] = "Lantern";
        }
    }

    private void Update()
    {
        if (!(pauseGameTime || PauseMenu.GameIsPaused) || panicMode)
        {
            if (!inArktis)
            {
                // I'm using this instead of Time.deltaTime to account for scene changes and lag
                gameTime += (Time.realtimeSinceStartup - lastUpdateTime);
                hourglassFill -= (Time.realtimeSinceStartup - lastUpdateTime) / timeMultiplier;
            } else
            {
                fileTime += (Time.realtimeSinceStartup - lastUpdateTime);
            }
        }

        lastUpdateTime = Time.realtimeSinceStartup;

        // starts panic mode at T-76.8 s
        // if timeMultiplier < 1, i.e. if hourglass is broken, start it earlier
        if (hourglassFill < panicTime / timeMultiplier && !panicMode)
        {
            panicMode = true;
            AudioManager.instance.ReloadMusic("Fragmentation");
        }

        if (hourglassFill < 0f)
        {
            EndRun();
        }
    }

    public int fileNumber;
    public string fileName;
    public float fileTime;
    public int fileCompletion;
    public int loopNumber = 0;
    public int plotProgress = 0;

    private float lastUpdateTime = 0f;
    public bool pauseGameTime = false;
    public float gameTime;

    public string previousLevel;
    public float playerMaxHealth = 10;
    public float playerCurrentHealth = 10;

    public float hourglassFill = 900f; // normally 900
    public float hourglassCapacity = 900f;
    public bool panicMode = false;
    public bool hourglassBroken = false;

    private float timeMultiplier = 1f; // decrease to die faster, increase to die slower

    public const float panicTime = 76.8f;

    public bool inArktis = true;

    public List<string> currentItems;
    public List<string> tools;
    public List<string> items;

    public List<string> playedCutscenes = new();
    public List<string> playedLines = new();
    public SerializableDictionary<string, string> currentStories = new();

    public List<string> foundItems = new();
    public List<string> permanentFoundItems = new();
    public List<bool> doorStates = new();

    public int money = 30;

    public List<NPCData> npcMemory = new();
    public List<ShopData> shopMemory = new();

    private SpawnPlayer currentRespawnPoint;

    public List<bool> uniqueEnemies = new();
    public SlainEnemies slainEnemies;

    public List<string> foundBadges = new();

    public List<TitleScreenTheme.TitleTheme> unlockedThemes = new();

    private GameUI_Controller guic;

    private static string[] keepDialogueOnLoop =
    {
        "seal_pile", "lighthouse_unlock"
    };
}

