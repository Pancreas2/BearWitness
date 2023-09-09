using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerEnemy : BaseEnemy
{
    private int facingDirection = 1;
    private float moveSpeed = 1.2f;
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    [SerializeField] private Transform turnCheckPoint;
    [SerializeField] private LayerMask m_WhatIsGround;

    void FixedUpdate()
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
