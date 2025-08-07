using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class DecayParticles : MonoBehaviour
{

    private ParticleSystem.EmissionModule module;

    private GameManager gameManager;


    void Start()
    {
        module = gameObject.GetComponent<ParticleSystem>().emission;
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (gameManager.panicMode)
        {
            float rate = GameManager.panicTime - gameManager.hourglassFill;
            module.rateOverTime = rate;

            if (!module.enabled) module.enabled = true;
        } else
        {
            module.enabled = false;
        }
    }
}
