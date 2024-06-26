using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value;
    private bool collected;

    private WalletUI wallet;

    private void Start()
    {
        wallet = FindObjectOfType<WalletUI>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !collected)
        {
            collected = true;
            GameManager.instance.money += value;
            wallet.AddMoney(value);
            Destroy(gameObject);
        }
    }
}
