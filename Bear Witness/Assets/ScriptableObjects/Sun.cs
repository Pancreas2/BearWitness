using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    GameManager gameManager;
    SpriteRenderer renderer;

    [SerializeField] private float dayLength = 600f;
    [SerializeField] private float xAmp = 4.5f;
    [SerializeField] private float yAmp = 4f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float periodicTime = gameManager.gameTime * 2 * Mathf.PI / dayLength;

        // one day occurs every 1440 game minutes
        float xpos = xAmp * Mathf.Cos(periodicTime);
        float ypos = yAmp * Mathf.Sin(periodicTime);
        transform.localPosition = new Vector2(xpos, ypos);

        // change to red during sunsets
        float altColors = (255f - (Mathf.Pow(Mathf.Cos(periodicTime), 4f) * 75f)) / 255f;
        Color newColor = new(1f, altColors, altColors);
        renderer.color = newColor;
    }
}
