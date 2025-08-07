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
        if (GameManager.instance.tools.Contains(item.name) || GameManager.instance.ContainsItem(item.name))
        {
            OnHasItem.Invoke();
        } else
        {
            OnNotHasItem.Invoke();
        }
    }

}
