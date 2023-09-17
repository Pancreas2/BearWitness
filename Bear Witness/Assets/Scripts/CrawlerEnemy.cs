using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerEnemy : MonoBehaviour
{
    private int facingDirection = 1;
    private readonly float moveSpeed = 1.2f;
    private Vector3 m_Velocity = Vector3.zero;
    [SerializeField] private Transform turnCheckPoint;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private bool doesContactDamage = false;
    private bool hiding = false;
    [SerializeField] private BaseEnemy baseEnemy;
    private Rigidbody2D m_Rigidbody2D;
    private Animator animator;
    private PlayerController player;

    public void Initialize()
    {
        m_Rigidbody2D = baseEnemy.m_Rigidbody2D;
        animator = baseEnemy.animator;
        player = baseEnemy.player;
    }

    void FixedUpdate()
    {
        if (!hiding)
        {
            Vector3 targetVelocity = new Vector3(moveSpeed * facingDirection, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, 0.05f);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(turnCheckPoint.position, 0.2f, m_WhatIsGround);
            if (colliders.Length > 0)
            {
                facingDirection *= -1;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (doesContactDamage && !collision.otherCollider.CompareTag("Ground"))
            {
                player.Damage(1, transform.position.x);
            }

            Retreat();
        }
    }

    public void WakeUp()
    {
        hiding = false;
        doesContactDamage = true;
    }

    public void Retreat()
    {
        animator.SetTrigger("hide");
        hiding = true;
        doesContactDamage = false;
    }
}
