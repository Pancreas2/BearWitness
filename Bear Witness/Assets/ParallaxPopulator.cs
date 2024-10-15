using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxPopulator : MonoBehaviour
{
    public GameObject mainCamera;
    [SerializeField] private List<ParallaxScroller> scrollers = new();

    void Awake()
    {
        foreach (ParallaxScroller scroller in scrollers)
        {
            scroller.cam = mainCamera;
        }
    }
}
