using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicTrigger : MonoBehaviour
{
    public UnityEvent OnTrigger;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            OnTrigger.Invoke();
        }
    }
}