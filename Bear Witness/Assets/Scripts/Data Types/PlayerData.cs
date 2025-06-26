using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

[System.Serializable]
public class PlayerData
{
    public float playerMaxHealth = 10;
    public List<string> currentItems;
    public List<string> tools;
    public List<string> items;

    public List<string> playedCutscenes = new();
    public List<string> playedLines = new();
    public List<string> foundItems = new();
    public List<string> permanentFoundItems = new();
    public SerializableDictionary<string, string> currentStories = new();
    public List<bool> doorStates = new();
    public List<NPCData> npcMemory = new();

    public string fileName;
    public float fileTime;
    public int fileCompletion;
    public int loopNumber;
    public int plotProgress;

    public List<ShopData> shopMemory = new();

    public List<string> foundBadges = new();

    public float gameTime;
    public float hourglassFill = 900f;
    public float hourglassCapacity = 900f;
    public int money;

    public string previousLevel;

    public List<bool> uniqueEnemies = new();
    public SlainEnemies slainEnemies;
    public List<TitleScreenTheme.TitleTheme> unlockedThemes = new();

    public PlayerData(GameManager gameManager)
    {
        playerMaxHealth = gameManager.playerMaxHealth;
        currentItems = gameManager.currentItems;
        tools = gameManager.tools;
        items = gameManager.items;

        playedCutscenes = gameManager.playedCutscenes;
        playedLines = gameManager.playedLines;
        foundItems = gameManager.foundItems;
        permanentFoundItems = gameManager.permanentFoundItems;
        currentStories = gameManager.currentStories;
        doorStates = gameManager.doorStates;
        npcMemory = gameManager.npcMemory;

        fileName = gameManager.fileName;
        fileTime = gameManager.fileTime;
        fileCompletion = gameManager.fileCompletion;
        loopNumber = gameManager.loopNumber;
        plotProgress = gameManager.plotProgress;

        hourglassCapacity = gameManager.hourglassCapacity;
        hourglassFill = gameManager.hourglassFill;
        gameTime = gameManager.gameTime;
        money = gameManager.money;

        previousLevel = gameManager.previousLevel;
        slainEnemies = gameManager.slainEnemies;
        shopMemory = gameManager.shopMemory;
        foundBadges = gameManager.foundBadges;
        unlockedThemes = gameManager.unlockedThemes;
    }
}
