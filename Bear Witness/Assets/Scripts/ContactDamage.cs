using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    private PlayerController player;
    public UnityEvent OnDamage;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player.Damage(damageAmount, transform.position.x);
            OnDamage.Invoke();
        }
    }
}
