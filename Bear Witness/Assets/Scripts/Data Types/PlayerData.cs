using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float playerMaxHealth = 5;
    public List<Item> currentItems;
    public List<Item> inventory;
    public int inventoryNextSpace = 0;
    public List<string> playedCutscenes = new();
    public List<string> playedLines = new();
    public List<string> foundItems = new();
    public List<string> permanentFoundItems = new();

    public List<NPCData> npcMemory = new();

    public string fileName;
    public float fileTime;
    public int fileCompletion;

    public List<ShopData> shopMemory = new();

    public List<Item> foundBadges = new();

    public float gameTime;

    public int money;

    public string previousLevel;

    public SlainEnemies slainEnemies;

    public PlayerData(GameManager gameManager)
    {
        playerMaxHealth = gameManager.playerMaxHealth;
        currentItems = gameManager.currentItems;
        inventory = gameManager.tools;
        playedCutscenes = gameManager.playedCutscenes;
        playedLines = gameManager.playedLines;
        foundItems = gameManager.foundItems;
        permanentFoundItems = gameManager.permanentFoundItems;
        npcMemory = gameManager.npcMemory;
        fileName = gameManager.fileName;
        fileTime = gameManager.fileTime;
        fileCompletion = gameManager.fileCompletion;
        gameTime = gameManager.gameTime;
        money = gameManager.money;
        previousLevel = gameManager.previousLevel;
        slainEnemies = gameManager.slainEnemies;
        shopMemory = gameManager.shopMemory;
        foundBadges = gameManager.foundBadges;
    }
}
