using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    GameManager gameManager;
    SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float periodicTime = gameManager.gameTime * Mathf.PI / 720f;

        // one day occurs every 1440 game minutes
        float xpos = -45f * Mathf.Cos(periodicTime);
        float ypos = 40f * Mathf.Sin(periodicTime) - 10f;
        transform.position = new Vector3(xpos, ypos, transform.position.z);

        // change to red during sunsets
        float altColors = (255f - (Mathf.Pow(Mathf.Cos(periodicTime), 4f) * 75f)) / 255f;
        Color newColor = new(1f, altColors, altColors);
        renderer.color = newColor;
    }
}
