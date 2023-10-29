using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchFace : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private RectTransform hourHand;
    [SerializeField] private Image calendar;

    [SerializeField] private Sprite[] calendarDays;


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Update();
    }

    void Update()
    {
        float timeInMinutes = gameManager.gameTime;
        hourHand.rotation = Quaternion.Euler(0f, 0f, 90f - timeInMinutes / 4f);
        int timeInDays = Mathf.Min(Mathf.FloorToInt(timeInMinutes / 1440f), 6);
        calendar.sprite = calendarDays[timeInDays];
    }
}
