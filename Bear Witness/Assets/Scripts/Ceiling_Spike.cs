using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceiling_Spike : MonoBehaviour
{
    public Collider2D senseRange;
    public Collider2D hurtbox;
    public Rigidbody2D rigidbody;

    private bool falling = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (falling)
            {
                FindObjectOfType<PlayerController>().Damage(1, transform.position.x);
                Destroy(gameObject);
            } else
            {
                senseRange.enabled = false;
                hurtbox.enabled = true;
                rigidbody.WakeUp();
                falling = true;
            }
        } else if (falling)
        {
            Destroy(gameObject);
        }
    }
}
