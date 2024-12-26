using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckForItem : MonoBehaviour
{
    [SerializeField] private Item item;

    public UnityEvent OnHasItem;
    public UnityEvent OnNotHasItem;

    void Start()
    {
        if (GameManager.instance.tools.Contains(item) || GameManager.instance.items.Contains(item))
        {
            OnHasItem.Invoke();
        } else
        {
            OnNotHasItem.Invoke();
        }
    }

}
