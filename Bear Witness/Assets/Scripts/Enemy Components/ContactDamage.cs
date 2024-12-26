using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    private PlayerController player;
    public UnityEvent OnDamage;
    public bool active = true;

    public Collider2D collider;

    public bool damageEnemies = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckDamage(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckDamage(collision);
    }

    private void CheckDamage(Collision2D collision)
    {
        if (active && collision.otherCollider.Equals(collider))
        {
            if (collision.collider.CompareTag("Player"))
            {
                player.Damage(damageAmount, transform.position.x);
                OnDamage.Invoke();
            }
            else if (damageEnemies)
            {
                collision.collider.TryGetComponent<ReceiveDamage>(out ReceiveDamage receiveDamage);
                if (receiveDamage) receiveDamage.Damage(damageAmount, transform.position.x);
            }
        }
    }

    public void SetActive(bool value)
    {
        active = value;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
