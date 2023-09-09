using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private int maxHealth = 5;
    private int currentHealth;
    [SerializeField] private bool doesContactDamage = false;
    private PlayerController player;

    private void Start()
    {
        currentHealth = maxHealth;
        player = FindObjectOfType<PlayerController>();
    }

    public void Damage(int damageValue)
    {
        currentHealth -= damageValue;
        if (currentHealth <= 0)
        {
            Perish();
        }
    }

    private void Perish() 
    {
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (doesContactDamage && collision.collider.CompareTag("Player"))
        {
            player.Damage(1, transform.position.x);
        }
    }
}
