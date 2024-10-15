using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollZoom : MonoBehaviour
{

    float zoomValue = 1f;

    void Update()
    {
        float scrollValue = Input.GetAxisRaw("Scroll");
        zoomValue += scrollValue;

        if (zoomValue < 0.5f) zoomValue = 0.5f;
        if (zoomValue > 5f) zoomValue = 5f;

        transform.localScale = new Vector3(zoomValue, zoomValue, zoomValue);
    }
}
