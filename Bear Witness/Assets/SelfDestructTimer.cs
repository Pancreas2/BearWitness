using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructTimer : MonoBehaviour
{
    [SerializeField] private float existenceTime;

    private float perishTime;

    void Start()
    {
        perishTime = Time.time + existenceTime;
    }

    void Update()
    {
        if (Time.time >= perishTime)
        {
            Destroy(gameObject);
        }
    }
}
