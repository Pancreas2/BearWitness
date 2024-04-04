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

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (active && collision.collider.CompareTag("Player") && collision.otherCollider.Equals(collider))
        {
            player.Damage(damageAmount, transform.position.x);
            OnDamage.Invoke();
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
