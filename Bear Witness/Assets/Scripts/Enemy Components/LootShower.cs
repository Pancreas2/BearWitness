using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootShower : MonoBehaviour
{
    public List<Item> loot = new();
    public GameObject carrier;

    public void SpawnLoot()
    {
        foreach (Item item in loot)
        {
            GameObject itemCarrier = Instantiate(carrier);
            itemCarrier.transform.position = transform.position;
            Vector2 randomVel = new(Random.value * 2f - 1f, Random.value * 4f);
            itemCarrier.GetComponent<Rigidbody2D>().velocity = randomVel;
            itemCarrier.GetComponent<ItemInteractable>().item = item;
            itemCarrier.GetComponent<SpriteRenderer>().sprite = item.image;
        }
    }
}
