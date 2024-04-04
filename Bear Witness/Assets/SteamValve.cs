using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamValve : MonoBehaviour
{

    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private float timeOffset;
    [SerializeField] private ContactDamage contactDamage;
    [SerializeField] private float nonEmittingTime = 1.5f;

    private float changeStateTime;
    private bool emitting = false;
    // Start is called before the first frame update
    void Start()
    {
        changeStateTime = timeOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= changeStateTime)
        {
            if (emitting)
            {
                changeStateTime = Time.time + nonEmittingTime;
            } else
            {
                changeStateTime = Time.time + 1f;
                particleSystem.Play();
            }

            emitting = !emitting;
            contactDamage.active = emitting;
        }   
    }
}
