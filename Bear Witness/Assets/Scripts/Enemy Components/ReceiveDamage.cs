using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDamage : MonoBehaviour
{
    [SerializeField] private ReceiveDamage passDamageTo;
    [SerializeField] private Renderer renderer;
    public float flashValue = 0f;

    [SerializeField] private List<Renderer> otherRenderers;

    public virtual void Damage(int damage, float sourceX, bool bypassInv = false)
    {
        flashValue = 2f;
        if (passDamageTo) passDamageTo.Damage(damage, sourceX, bypassInv);
    }

    private void FixedUpdate()
    {
        if (flashValue != 0f)
        {
            flashValue = Mathf.Max(flashValue - 10 * Time.deltaTime, 0);
            if (renderer) renderer.material.SetFloat("_FlashBrightness", flashValue);

            foreach (Renderer r in otherRenderers)
            {
                r.material.SetFloat("_FlashBrightness", flashValue);
            }
        }
    }

    public int GetHealth()
    {
        if (passDamageTo)
        {
            passDamageTo.TryGetComponent<BaseEnemy>(out BaseEnemy enemy);
            if (enemy) return enemy.currentHealth;
            else return 0;
        }
        else return 0;
    }
}
