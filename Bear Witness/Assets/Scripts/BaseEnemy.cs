using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEnemy : MonoBehaviour
{
    private readonly int maxHealth = 50;
    private int currentHealth;
    public PlayerController player;
    public Animator animator;
    [SerializeField] private bool doKnockback = true;
    public Rigidbody2D m_Rigidbody2D;

    public UnityEvent OnHurt;
    public UnityEvent OnStart;

    private void Start()
    {
        currentHealth = maxHealth;
        player = FindObjectOfType<PlayerController>();
        OnStart.Invoke();
    }

    public void Damage(int damageValue, float sourcePosX)
    {
        currentHealth -= damageValue;
        if (currentHealth <= 0)
        {
            Perish();
        }
        OnHurt.Invoke();
        m_Rigidbody2D.velocity = Vector2.zero;
        Vector2 knockbackForce = new((transform.position.x - sourcePosX) * 20f, 100f);
        m_Rigidbody2D.AddForce(knockbackForce);
    }

    private void Perish() 
    {
        Destroy(gameObject);
    }
}
