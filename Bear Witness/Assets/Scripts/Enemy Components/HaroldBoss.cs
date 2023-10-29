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
    public Collider2D physicsCollider;
    private string lastState = "roar";

    public UnityEvent OnFightStart;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        AnimatorStateInfo animIndex = animator.GetCurrentAnimatorStateInfo(0);
        if (animIndex.IsName("roar"))
        {
            rb.WakeUp();
            OnFightStart.Invoke();
            lastState = "roar";
        } else if (animIndex.IsName("rush"))
        {
            RushAttack();
            lastState = "rush";
        } else if (animIndex.IsName("slam"))
        {
            SlamAttack();
            lastState = "slam";
        } else if (animIndex.IsName("slam_seek")) {
            SlamSeek();
            lastState = "slam_seek";
        } else if (animIndex.IsName("base"))
        {
            SelectAttack();
            lastState = "base";
        }
    }

    void RushAttack()
    {
        physicsCollider.enabled = true;
        Vector2 targetVelocity = new Vector2(facingDirection * 350f * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref defaultVelocity, 0.05f);
        if ((arenaCenter.position.x - transform.position.x) * facingDirection < -6f)
        {
            animator.SetFloat("randomAttack", 2f);
            animator.SetTrigger("attackDone");
            rb.AddForce(Vector2.down * 2000f);
        }
    }

    void SlamAttack() {
        Debug.Log("slam poetry");
    }

    void SlamSeek()
    {
        physicsCollider.enabled = false;
        Vector2 targetVelocity = new Vector2(Mathf.Sign(player.position.x - rb.position.x) * 3f, 0f);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref defaultVelocity, 0.05f);
    }

    void SelectAttack()
    {
        float randy = Random.value;
        animator.SetFloat("randomAttack", randy);
        if (randy < 0.5f) facingDirection *= -1;
        animator.SetTrigger("attackStart");
    }
}
