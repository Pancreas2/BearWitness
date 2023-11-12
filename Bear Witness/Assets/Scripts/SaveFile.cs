using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveFile : MonoBehaviour
{
    private GameManager gameManager;
    private string lastSavedSceneName = "Arktis_Den";
    [SerializeField] private int slotID;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI location;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI percent;

    [SerializeField] private TextMeshProUGUI createText;

    private string file;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        file = SaveSystem.LoadPlayer(slotID);
        if (file == null)
        {
            createText.gameObject.SetActive(true);
            nameText.gameObject.SetActive(false);
            location.gameObject.SetActive(false);
            time.gameObject.SetActive(false);
            percent.gameObject.SetActive(false);
        }
        else
        {
            createText.gameObject.SetActive(false);

            GameManager temporaryGameManager = new();
            JsonUtility.FromJsonOverwrite(file, temporaryGameManager);

            Debug.Log(temporaryGameManager.previousLevel);

            lastSavedSceneName = temporaryGameManager.previousLevel;
            nameText.text = temporaryGameManager.fileName;
            location.text = lastSavedSceneName;  // find a good way to convert this
            time.text = TimeConverter(temporaryGameManager.fileTime);
            percent.text = temporaryGameManager.fileCompletion.ToString() + "%";

            Destroy(temporaryGameManager);

            nameText.gameObject.SetActive(true);
            location.gameObject.SetActive(true);
            time.gameObject.SetActive(true);
            percent.gameObject.SetActive(true);
        }
    }

    public void LoadFile()
    {
        if (file == null)
        {
            gameManager.fileNumber = slotID;

            if (slotID == 3)
                // Tutorial replay
                gameManager.ChangeScene("Arktis_Den");
            else gameManager.ChangeScene("Shores_Entrance");
        }
        else
        {
            gameManager.LoadPlayerData(slotID);
            gameManager.fileNumber = slotID;
            gameManager.ChangeScene(lastSavedSceneName);
        }
    }

    private string TimeConverter(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600f);
        time -= hours * 3600f;
        int minutes = Mathf.FloorToInt(time / 60);
        time -= minutes * 60f;
        int seconds = Mathf.FloorToInt(time);
        string returnString = hours.ToString() + ":";
        if (minutes < 10)
        {
            returnString += "0";
        }
        returnString += minutes + ":";
        if (seconds < 10)
        {
            returnString += "0";
        }
        returnString += seconds;
        return returnString;
    }
}
