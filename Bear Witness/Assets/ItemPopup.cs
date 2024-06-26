using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopup : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;

    private Queue<Item> items = new();
    private float delay = 2.5f;
    private float activeTimer = 0f;

    private void Update()
    {
        if (activeTimer <= Time.time && items.Count > 0)
        {
            activeTimer = Time.time + delay;
            Item targetItem = items.Dequeue();
            ShowItem(targetItem);
        }
    }

    private void ShowItem(Item item)
    {
        image.sprite = item.image;
        text.text = item.name;
        animator.SetTrigger("Activate");
    }

    public void QueueItem(Item item)
    {
        items.Enqueue(item);
    } 
}
