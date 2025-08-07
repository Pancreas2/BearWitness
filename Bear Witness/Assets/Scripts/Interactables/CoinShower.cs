using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinShower : MonoBehaviour
{
    public GameObject bigCoin;
    public GameObject mediumCoin;
    public GameObject smallCoin;

    public void SpawnCoins(int sum)
    {
        // BADGE EFFECT: AVARICE
        if (GameManager.instance.currentBadges.Contains("Avarice"))
        {
            sum = Mathf.RoundToInt(sum * 1.25f);
        }

        // has a chance to produce more than the optimal number of coins
        int bigCoins = Mathf.FloorToInt(sum / 16f * Random.Range(0.35f, 1f));
        sum -= bigCoins * 16;
        int midCoins = Mathf.FloorToInt(sum / 4f * Random.Range(0.35f, 1f));
        sum -= midCoins * 4;
        // remainder is the number of small coins
        for (int i = 0; i < bigCoins; i++)
        {
            GameObject coin = Instantiate(bigCoin);
            coin.transform.position = transform.position;
            Vector2 randomVel = new(Random.value * 2f - 1f, Random.value * 4f);
            coin.GetComponent<Rigidbody2D>().velocity = randomVel;
        }
        for (int i = 0; i < midCoins; i++)
        {
            GameObject coin = Instantiate(mediumCoin);
            coin.transform.position = transform.position;
            Vector2 randomVel = new(Random.value * 2f - 1f, Random.value * 4f);
            coin.GetComponent<Rigidbody2D>().velocity = randomVel;
        }
        for (int i = 0; i < sum; i++)
        {
            GameObject coin = Instantiate(smallCoin);
            coin.transform.position = transform.position;
            Vector2 randomVel = new(Random.value * 2f - 1f, Random.value * 4f);
            coin.GetComponent<Rigidbody2D>().velocity = randomVel;
        }
    }
}
