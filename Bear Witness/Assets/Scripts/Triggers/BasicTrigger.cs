using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicTrigger : MonoBehaviour
{
    public UnityEvent OnTrigger;
    [SerializeField] private float deadTime = 0.5f;
    private float isActiveTime = 0f;

    [SerializeField] private string contactTag = "Player";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActiveTime <= Time.time)
        {
            if (collision.collider.CompareTag(contactTag))
            {
                isActiveTime = Time.time + deadTime;
                OnTrigger.Invoke();
            }
        }

    }
}
