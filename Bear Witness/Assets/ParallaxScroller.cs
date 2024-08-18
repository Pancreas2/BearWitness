using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    private float startposX;
    private float startposY;
    public bool scrollX = true;
    public float centerPointX;
    public float parallaxFactorX;
    public bool scrollY;
    public float centerPointY;
    public float parallaxFactorY;
    public GameObject cam;

    void Start()
    {
        startposX = transform.position.x;
        startposY = transform.position.y;
    }

    void Update()
    {
        if (scrollX)
        {
            float distance = (cam.transform.position.x - centerPointX) * parallaxFactorX;
            Vector3 newPosition = new Vector3(startposX + distance, transform.position.y);
            transform.position = newPosition;
        }

        if (scrollY)
        {
            float distance = (cam.transform.position.y - centerPointY) * parallaxFactorY;
            Vector3 newPosition = new Vector3(transform.position.x, startposY + distance);
            transform.position = newPosition;
        }
    }
}
