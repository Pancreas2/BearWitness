using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    private int currentHealth;
    public Animator animator;
    [SerializeField] private bool doKnockback = true;
    [SerializeField] private Vector2 knockback;
    public Rigidbody2D m_Rigidbody2D;
    public bool invulnerable = false;

    public UnityEvent OnHurt;
    public UnityEvent OnHurtInvulnerable;
    public UnityEvent OnStart;
    public UnityEvent OnPerish;

    private void Start()
    {
        currentHealth = maxHealth;
        OnStart.Invoke();
    }

    public void Damage(int damageValue, float sourcePosX)
    {
        if (!invulnerable)
        {
            if (currentHealth > 0)
            {
                currentHealth -= damageValue;
                if (currentHealth <= 0)
                {
                    Perish();
                } else
                {
                    OnHurt.Invoke();
                }
            }
        } else
        {
            OnHurtInvulnerable.Invoke();
        }

        if (currentHealth > 0)
        {
            m_Rigidbody2D.velocity = Vector2.zero;
            Vector2 knockbackForce = new((transform.position.x - sourcePosX) * knockback.x, knockback.y);
            m_Rigidbody2D.AddForce(knockbackForce);
        }

    }

    private void Perish() 
    {
        OnPerish.Invoke();
    }
}
