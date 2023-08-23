using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceiling_Spike : MonoBehaviour
{
    public Collider2D senseRange;
    public Collider2D hurtbox;
    public Rigidbody2D rigidbody;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.otherCollider == hurtbox)
            {
                FindObjectOfType<PlayerController>().Damage(1, transform.position.x);
                Destroy(gameObject);
            } else
            {
                senseRange.enabled = false;
                rigidbody.WakeUp();
            }
        } else if (collision.collider.CompareTag("Ground") && collision.otherCollider == hurtbox)
        {
            Destroy(gameObject);
        }
    }
}
