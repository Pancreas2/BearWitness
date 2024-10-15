using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDamage : MonoBehaviour
{
    [SerializeField] private ReceiveDamage passDamageTo;
    [SerializeField] private Renderer renderer;
    public float flashValue = 0f;

    public virtual void Damage(int damage, float sourceX)
    {
        flashValue = 2f;
        if (passDamageTo) passDamageTo.Damage(damage, sourceX);
    }

    private void FixedUpdate()
    {
        if (flashValue != 0f)
        {
            flashValue = Mathf.Max(flashValue - 10 * Time.deltaTime, 0);
            if (renderer) renderer.material.SetFloat("_FlashBrightness", flashValue);
        }
    }
}
