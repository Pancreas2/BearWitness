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
            tools.Add(item.name);

            if (tools.Count <= 1)
            {
                GameObject tutorial = Resources.Load<GameObject>("FirstToolCanvas");
                Instantiate(tutorial);
            }

            // render in menu
            //foreach (InventorySlot toolSlot in invManager.toolSlots)
            //{
            //    if (toolSlot)
            //        toolSlot.FindItem();
            //}
        } else if (item.type == Item.ItemType.Item)
        {
            bool placedItem = false;
            foreach (ItemStack stack in items)
            {
                if (stack.item == item.name || stack.empty)
                {
                    stack.Increment(item.name);
                    placedItem = true;
                    break;
                }
            }

            if (!placedItem)
            {
                ItemStack newStack = new ItemStack();
                newStack.Increment(item.name);
                Debug.Log(newStack);
                items.Add(newStack);
            }

            // render in menu
            //foreach (InventorySlot itemSlot in invManager.itemSlots)
            //{
            //    itemSlot.FindItem();
            //}
        } else if (item.type == Item.ItemType.Key)
        {
            keys.Add(item.name);
        }

        // achievements:
        switch (item.name)
        {
            case "Ice Pick":
                GrantAchievement("Familiar");
                break;
            case "Lantern":
                GrantAchievement("Prometheus");
                break;
            case "Phaser":
                GrantAchievement("Recoil");
                break;
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

    public void Consume(string item)
    {
        if (ContainsItem(item))
        {
            items.Find(target => target.item == item).Decrement();
        }
        else Debug.LogError("Attempted to consume " + item + ", but none exists");
    }

    public void DamagePlayer(float damage)
    {
        if (!guic) guic = FindObjectOfType<GameUI_Controller>();

        damage /= GetDefenseModifiers();  // reduces damage, but never to 0
        Debug.Log(GetDefenseModifiers());

        playerCurrentHealth -= damage;
        guic.hourglass.DamageFlash(damage);

        if (playerCurrentHealth <= 0)
        {
            AudioManager.instance.Play("Shatter", 0, 0, 0);
            guic.hourglass.SetBroken(true);
            if (recordingSegment)
            {
                currentSegment.AddPOI(gameTime, "Shatter");
                recDamage++;
            }

            if (panicMode)
            {
                StartCoroutine(PlayerDies());
            }

            timeMultiplier /= shatterMultiplier;
        }
        else
        {
            AudioManager.instance.Play("TakeDamage", 0, 0, 0);
            if (recordingSegment)
            {
                currentSegment.AddPOI(gameTime, "Damage");
                recDamage++;
            }
        }
    }

    public void HealPlayer(float heal)
    {
        float mod = 1f;
        if (currentBadges.Contains("Afishionado")) mod *= 1.5f;

        playerCurrentHealth = Mathf.Min(playerCurrentHealth + heal * mod, playerMaxHealth);
        if (playerCurrentHealth > 0) timeMultiplier = 1f;  // if you heal after shattering, you return to the normal time loss rate  <- do I want this?

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

        gameTime = 0f;

        StartSegmentRecording();
        currentSegment.AddPOI(gameTime, "Start");

        panicMode = false;
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

    IEnumerator PlayerDies()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.Kablooie();
        player.GetComponent<PlayerMovement>().Freeze("death");
        yield return new WaitForSeconds(2f);
        EndRun();
    }

    public void EndRun()
    {
        if (!guic) guic = GameUI_Controller.instance;

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

        fileTime += gameTime;
        gameTime = 0f;
        inArktis = true;
        timeMultiplier = 1f;
        hourglassFill = hourglassCapacity;

        panicMode = false;
        dying = false;

        AudioManager.instance.InstantStopAll();

        ScrapSegment();

        playerCurrentHealth = playerMaxHealth;
        guic.hourglass.SetBroken(false);

        ChangeScene("The_Twixt");

        guic.HideAll();

        PlayerController player = FindObjectOfType<PlayerController>();
        if (tools.Contains("Broken Lantern"))
        {
            int index = tools.IndexOf("Broken Lantern");
            tools[index] = "Lantern";
        }
    }

    public void ExitTwixt()
    {
        guic.ShowAll();
        ChangeScene("Arktis_Den");

        int totalLoopsAllTime = PlayerPrefs.GetInt("TotalLoops");

        PlayerPrefs.SetInt("TotalLoops", totalLoopsAllTime + 1);

        if (totalLoopsAllTime == 0)
        {
            GrantAchievement("Again");
        } else if (totalLoopsAllTime == 9)
        {
            GrantAchievement("And Again");
        } else if (totalLoopsAllTime == 49)
        {
            GrantAchievement("And Again...");
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
            currentSegment.AddPOI(gameTime, "Fragment");
            panicMode = true;
            AudioManager.instance.ReloadMusic();
        }

        if (hourglassFill < 0f)
        {
            if (!dying) StartCoroutine(PlayerDies());
            dying = true;
        }

        List<TemporaryBuff> buffsToRemove = new();
        foreach (TemporaryBuff tempBuff in activeTempBuffs)
        {
            if (tempBuff.endTime < Time.time)
            {
                buffsToRemove.Add(tempBuff);
            }
        }

        foreach (TemporaryBuff tempBuff in buffsToRemove)
        {
            activeTempBuffs.Remove(tempBuff);
        }
    }

    public void StartSegmentRecording(string bench = "Start")
    {
        if (!recordingSegment)
        {
            currentSegment = new();
            currentSegment.startBench = bench;
            recDamage = 0;
            recInitialSand = hourglassFill;
            recInitialTime = gameTime;
            currentSegment.startTime = gameTime;
            recordingSegment = true;
        }
    }

    public void ScrapSegment()
    {
        currentSegment = new();
        recordingSegment = false;
    }

    public void SaveCurrentSegment(string bench)
    {
        PrepSegmentForSave(bench);
        savedSegments.Add(currentSegment);
    }

    public void PrepSegmentForSave(string bench)
    {
        currentSegment.damageTaken = recDamage;
        currentSegment.sandLost = hourglassFill - recInitialSand;
        currentSegment.elapsedTime = gameTime - recInitialTime;
        currentSegment.endBench = bench;
    }

    public RunSegment GetSegment()
    {
        return currentSegment;
    }

    public void AddPOIToSegment(string type)
    {
        currentSegment.AddPOI(gameTime, type);
    }

    public float GetAttackModifiers()
    {
        float mod = 1f;
        if (BuffActive(TemporaryBuff.BuffType.Fury)) mod *= 1.5f;
        if (currentBadges.Contains("First Strike") && playerCurrentHealth == playerMaxHealth) mod *= 1.35f;
        if (currentBadges.Contains("Final Frenzy") && panicMode) mod *= 1.6f;
        if (currentBadges.Contains("Reckless")) mod *= 1.25f;
        return mod;
    }

    public float GetSpeedModifiers(PlayerController player)
    {
        float mod = 1f;
        if (BuffActive(TemporaryBuff.BuffType.Vigor)) mod *= 1.25f;
        if (currentBadges.Contains("Final Flight") && panicMode) mod *= 1.25f;
        if (currentBadges.Contains("Resonance") && Time.time - player.invTime < 1f) mod *= 1.25f;
        return mod;
    }

    public float GetDefenseModifiers()
    {
        float mod = 1f;
        if (BuffActive(TemporaryBuff.BuffType.Resilience)) mod *= 1.5f;
        return mod;
    }

    public void AddTempBuff(TemporaryBuff.BuffType type, float duration)
    {
        TemporaryBuff newBuff = new(type, Time.time + duration);
        Debug.Log(newBuff.endTime);
        activeTempBuffs.Add(newBuff);
    }

    public void GrantAchievement(string achievement)
    {
        bool hadAchievement = PlayerPrefs.GetInt(achievement, 0) == 1;
        PlayerPrefs.SetInt(achievement, 1);

        if (!hadAchievement)
        {
            Achievement ach = Resources.Load<Achievement>("Achievements/" + achievement);
            guic.AchievementPopup(ach);
        }
    }

    public bool BuffActive(TemporaryBuff.BuffType type)
    {
        foreach (TemporaryBuff buff in activeTempBuffs)
        {
            if (buff.buff == type)
            {
                return true;
            }
        }

        return false;
    }

    public bool ContainsItem(string name)
    {
        bool result = false;
        foreach (ItemStack stack in items)
        {
            if (name == stack.item)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    public int fileNumber;
    public string fileName;
    public float fileTime;
    public int fileCompletion;
    public int loopNumber = 0;
    public int plotProgress = 0;
    public List<string> arcasProgressionDialogues = new();

    private float lastUpdateTime = 0f;
    public bool pauseGameTime = false;
    public float gameTime;

    public string previousLevel;
    public float playerMaxHealth = 10f;
    public float playerCurrentHealth = 10f;

    public float hourglassFill = 900f; // normally 900
    public float hourglassCapacity = 900f;
    public bool panicMode = false;
    public bool hourglassBroken = false;

    public float timeMultiplier = 1f; // decrease to die faster, increase to die slower

    public const float panicTime = 76.8f;
    public const float shatterMultiplier = 1.5f;

    public bool inArktis = true;

    public List<string> currentItems;
    public List<string> tools;
    public List<ItemStack> items;
    public List<string> keys;

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
    public List<string> currentBadges = new();

    public List<TemporaryBuff> activeTempBuffs = new();

    public List<TitleScreenTheme.TitleTheme> unlockedThemes = new();

    private GameUI_Controller guic;

    private static string[] keepDialogueOnLoop =
    {
        "seal_pile", "lighthouse_unlock", "lighthouse_statue"
    };

    private bool recordingSegment = false;
    private RunSegment currentSegment = new();

    private int recDamage = 0;
    private float recInitialSand = 0;
    private float recInitialTime = 0;

    public List<RunSegment> savedSegments = new();

    [SerializeField] private GameObject firstToolPopup;

    bool dying = false;
}

