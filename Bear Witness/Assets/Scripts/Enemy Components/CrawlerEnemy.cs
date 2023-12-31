using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerEnemy : MonoBehaviour
{
    private int facingDirection = 1;
    [SerializeField] private float moveSpeed = 1.2f;
    private Vector3 m_Velocity = Vector3.zero;
    [SerializeField] private Transform turnCheckPoint;
    [SerializeField] private Transform ledgeCheckPoint;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private bool doesContactDamage = false;
    private bool hiding = false;
    [SerializeField] private BaseEnemy baseEnemy;
    private Rigidbody2D m_Rigidbody2D;
    private Animator animator;
    private PlayerController player;

    private bool inAir = false;

    private void Start()
    {
        m_Rigidbody2D = baseEnemy.m_Rigidbody2D;
        animator = baseEnemy.animator;
        player = FindObjectOfType<PlayerController>();
    }

    void FixedUpdate()
    {
        if (!hiding)
        {
            Vector3 targetVelocity = new Vector3(moveSpeed * facingDirection, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, 0.05f);

            Collider2D[] wallColliders = Physics2D.OverlapCircleAll(turnCheckPoint.position, 0.2f, m_WhatIsGround);
            if (wallColliders.Length > 0)
            {
                facingDirection *= -1;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }

            Collider2D[] ledgeColliders = Physics2D.OverlapCircleAll(ledgeCheckPoint.position, 0.05f, m_WhatIsGround);
            if (ledgeColliders.Length == 0 && !inAir)
            {
                facingDirection *= -1;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                inAir = true;
            } else if (ledgeColliders.Length > 0)
            {
                inAir = false;
            }
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
