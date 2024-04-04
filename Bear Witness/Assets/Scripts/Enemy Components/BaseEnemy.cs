using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEnemy : ReceiveDamage
{
    public int maxHealth = 50;
    public int currentHealth;
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

    private void FixedUpdate()
    {

    }

    public override void Damage(int damageValue, float sourcePosX)
    {
        if (!invulnerable)
        {
            if (currentHealth > 0)
            {
                DecreaseHealth(damageValue);
            }
        } else
        {
            OnHurtInvulnerable.Invoke();
        }

        if (currentHealth > 0)
        {
            m_Rigidbody2D.velocity = Vector2.zero;
            Vector2 knockbackForce = new(Mathf.Sign(transform.position.x - sourcePosX) * knockback.x, knockback.y);
            m_Rigidbody2D.AddForce(knockbackForce);
        }

    }

    virtual public void DecreaseHealth(int damageValue)
    {
        currentHealth -= damageValue;
        if (currentHealth <= 0)
        {
            Perish();
        }
        else
        {
            OnHurt.Invoke();
        }
    }

    virtual public void Perish() 
    {
        OnPerish.Invoke();
    }
}
