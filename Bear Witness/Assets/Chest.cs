using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : ReceiveDamage
{
    [SerializeField] private CoinShower coinShower;
    [SerializeField] private LootShower lootShower;
    [SerializeField] private Animator animator;

    [SerializeField] private int coinAmount;
    private bool opened = false;
    [SerializeField] private string chestID;

    private void Start()
    {
        opened = GameManager.instance.foundItems.Contains(chestID);
    }

    private void Update()
    {
        animator.SetBool("open", opened);
    }

    public override void Damage(int damage, float sourceX, bool bypassInv = false)
    {
        if (!opened)
        {
            opened = true;
            if (coinShower) coinShower.SpawnCoins(coinAmount);
            if (lootShower) lootShower.SpawnLoot();
        }
    }
}
