using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipScreen : MonoBehaviour
{

    [SerializeField] private List<Sprite> digitSprites;
    [SerializeField] private List<SpriteRenderer> digitRenderers;

    private List<int> convertedTime = new();
    private List<int> rememberedTime = new();

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
        Debug.Log(convertedTime.Count);
    }

    // Update is called once per frame
    void Update()
    {
        float gameTime = gameManager.gameTime;
        convertedTime[0] = Mathf.FloorToInt(gameTime / 1440f);
        gameTime -= convertedTime[0] * 1440f;
        convertedTime[1] = Mathf.FloorToInt(gameTime / 600f);
        gameTime -= convertedTime[1] * 600f;
        convertedTime[2] = Mathf.FloorToInt(gameTime / 60f);
        gameTime -= convertedTime[2] * 60f;
        convertedTime[3] = Mathf.FloorToInt(gameTime / 10f);
        gameTime -= convertedTime[3] * 10f;
        convertedTime[4] = Mathf.FloorToInt(gameTime);

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
