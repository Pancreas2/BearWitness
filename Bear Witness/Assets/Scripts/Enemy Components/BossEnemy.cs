using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    private BossHealthBar hpBar;
    [SerializeField] private BaseEnemy baseEnemy;

    private PlayerMovement player;

    void Start()
    {
        hpBar = FindObjectOfType<BossHealthBar>();
        player = FindObjectOfType<PlayerMovement>();
    }

    public void UpdateHealth()
    {
        hpBar.SetMaxHP(baseEnemy.maxHealth);
        hpBar.SetHPValue(baseEnemy.currentHealth);
        hpBar.SetVisibility(true);
    }

    public void DecreaseOnHurt()
    {
        hpBar.SetHPValue(Mathf.Max(baseEnemy.currentHealth, 0));
    }

    public void FreezePlayer()
    {
        player.frozen = true;
    }

    public void UnfreezePlayer()
    {
        player.frozen = false;
    }
}
