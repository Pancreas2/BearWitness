using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossEnemy : BaseEnemy
{
    private BossHealthBar hpBar;

    private PlayerMovement player;
    private AudioManager audioManager;

    [SerializeField] private string bossMusic;
    [SerializeField] private Sprite bossPortrait;

    [SerializeField] public UniqueEnemy uniqueEnemy;

    [HideInInspector] public bool fightActive = false;

    void Start()
    {
        currentHealth = maxHealth;
        hpBar = FindObjectOfType<BossHealthBar>();
        player = FindObjectOfType<PlayerMovement>();
        audioManager = FindObjectOfType<AudioManager>();
    }
    
    public virtual void BeginFight()
    {
        fightActive = true;
        audioManager = FindObjectOfType<AudioManager>();
        if (bossMusic != "")
        {
            audioManager.StopAll(0f);
            audioManager.Play(bossMusic, 0, 0.5f);
        }
        OnStart.Invoke();
    }

    public void UpdateHealth()
    {
        hpBar.SetMaxHP(maxHealth);
        hpBar.SetHPValue(currentHealth);
        hpBar.SetPortrait(bossPortrait);
        hpBar.SetVisibility(true);
    }

    public void FreezePlayer()
    {
        player.frozen = true;
    }

    public void UnfreezePlayer()
    {
        player.frozen = false;
    }

    override public void Perish()
    {
        fightActive = false;
        uniqueEnemy.UniqueEnemySlain();
        audioManager.Stop(bossMusic, 1f);
        hpBar.SetVisibility(false);
        base.Perish();
    }

    public override void DecreaseHealth(int damageValue)
    {
        base.DecreaseHealth(damageValue);
        hpBar.SetHPValue(Mathf.Max(currentHealth, 0));
    }
}
