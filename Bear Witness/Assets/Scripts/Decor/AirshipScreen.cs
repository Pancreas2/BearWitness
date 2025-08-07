using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipScreen : MonoBehaviour
{

    [SerializeField] private List<Sprite> digitSprites;
    [SerializeField] private List<SpriteRenderer> digitRenderers;

    [SerializeField] private Transform airshipIndicator;

    private List<int> convertedTime = new();
    private List<int> rememberedTime = new();

    readonly private float airshipIndicatorLeft = -0.875f;
    readonly private float airshipIndicatorRight = 0.875f;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        for (int i = 0; i < 5; i++)
        {
            convertedTime.Add(0);
            rememberedTime.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float gameTime = gameManager.gameTime;
        float displayTime = gameTime;

        float testTime = AppearAtTime.EventMatch[AppearAtTime.Events.AirshipLeaveShore];

        if (gameTime < testTime)
        {
            displayTime = testTime - gameTime;
            airshipIndicator.localPosition = new Vector2(airshipIndicatorLeft, airshipIndicator.localPosition.y);
        } else
        {
            testTime = AppearAtTime.EventMatch[AppearAtTime.Events.AirshipArriveCity];
            if (gameTime < testTime)
            {
                displayTime = testTime - gameTime;
                float dist = displayTime / (AppearAtTime.EventMatch[AppearAtTime.Events.AirshipArriveCity] - AppearAtTime.EventMatch[AppearAtTime.Events.AirshipLeaveShore]);
                airshipIndicator.localPosition = new Vector2(airshipIndicatorLeft * dist + airshipIndicatorRight * (1 - dist), airshipIndicator.localPosition.y);
            } else
            {
                airshipIndicator.localPosition = new Vector2(airshipIndicatorRight, airshipIndicator.localPosition.y);
            }
        }

        convertedTime[0] = Mathf.FloorToInt(displayTime / 1440f);
        displayTime -= convertedTime[0] * 1440f;
        convertedTime[1] = Mathf.FloorToInt(displayTime / 600f);
        displayTime -= convertedTime[1] * 600f;
        convertedTime[2] = Mathf.FloorToInt(displayTime / 60f);
        displayTime -= convertedTime[2] * 60f;
        convertedTime[3] = Mathf.FloorToInt(displayTime / 10f);
        displayTime -= convertedTime[3] * 10f;
        convertedTime[4] = Mathf.FloorToInt(displayTime);

        for (int i = 0; i < 5; i++)
        {
            digitRenderers[i].sprite = digitSprites[convertedTime[i]];
        }

        if (rememberedTime != convertedTime)
        {
            rememberedTime = convertedTime;
        }
    }
}
