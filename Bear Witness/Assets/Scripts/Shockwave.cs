using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public float lifetime = 1f;
    public float speed = 1f;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    private float deathTime = 0;

    void Awake()
    {
        rigidbody.velocity = new(speed, 0);
        deathTime = Time.time + lifetime;
        animator.speed = 1 / lifetime;
    }

    void Update()
    {
        if (Time.time >= deathTime || rigidbody.velocity.x == 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float value)
    {
        rigidbody.velocity = new(value, 0);
        if (value < 0) transform.localScale = new(-1, 1);
    }
}
