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
    private float invTime = 0f;

    public UnityEvent OnStart;
    public UnityEvent OnPerish;

    private bool deathRecorded = false;

    [SerializeField] private CoinShower coinShower;
    [SerializeField] private int droppedCoins = 0;

    private void Start()
    {
        currentHealth = maxHealth;
        OnStart.Invoke();
    }

    public override void Damage(int damageValue, float sourcePosX, bool bypassInv = false)
    {
        if (bypassInv || (invTime < Time.time && !invulnerable))
        {
            flashValue = 2f;
            invTime = Time.time + 0.2f;
            if (currentHealth > 0)
            {
                AudioManager.instance.Play("Hit", 0, 0, 0);
                DecreaseHealth(damageValue);
            }
        } else if (invulnerable)
        {
            AudioManager.instance.Play("HitInv", 0, 0, 0);
        }

        if (currentHealth > 0 && m_Rigidbody2D)
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
    }

    virtual public void Perish() 
    {
        if (coinShower) coinShower.SpawnCoins(droppedCoins);
        TryGetComponent<AttackBounce>(out AttackBounce atkBounce);
        if (atkBounce) atkBounce.SetActive(false);
        OnPerish.Invoke();
        if (!deathRecorded)
        {
            int counter = PlayerPrefs.GetInt("EnemiesDefeated", 0);
            deathRecorded = true;
            PlayerPrefs.SetInt("EnemiesDefeated", counter + 1);

            if (counter == 9)
            {
                GameManager.instance.GrantAchievement("Brawler");
            } else if (counter == 99)
            {
                GameManager.instance.GrantAchievement("Crusher");
            }
        }
    }
}
