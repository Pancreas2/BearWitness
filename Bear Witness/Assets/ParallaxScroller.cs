using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    private Vector2 length = new(1f, 1f), startpos;
    public GameObject camera;
    public Vector2 parallax;
    public bool loopX, loopY;

    public Vector2 overrideLength;

    private void Start()
    {
        startpos = transform.position;
        if (overrideLength.x != 0 || overrideLength.y != 0)
            length = overrideLength;
        else
        {
            TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer);
            if (renderer)
                length = renderer.bounds.size;
        }
    }

    private void Update()
    {
        Vector2 temp = camera.transform.position * (new Vector2(1, 1) - parallax);
        Vector2 distance = (camera.transform.position * parallax);

        transform.position = startpos + distance;

        if (loopX)
        {
            if (temp.x > (startpos + length).x) startpos.x += length.x;
            else if (temp.x < (startpos - length).x) startpos.x -= length.x;
        }

        if (loopY)
        {
            if (temp.y > (startpos + length).y) startpos.y += length.y;
            else if (temp.y < (startpos - length).y) startpos.y -= length.y;
        }

    }
}
