using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HaroldBoss : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private int facingDirection = 1;
    public Transform arenaCenter;
    public Transform player;
    Vector3 defaultVelocity = Vector3.zero;

    public UnityEvent OnFightStart;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("roar"))
        {
            rb.WakeUp();
            OnFightStart.Invoke();
        } else if (animator.GetCurrentAnimatorStateInfo(0).IsName("rush"))
        {
            RushAttack();
        }
    }

    void RushAttack()
    {
        Vector2 targetVelocity = new Vector2(facingDirection * 350f * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref defaultVelocity, 0.05f);
    }
}
