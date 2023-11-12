using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningEnemy : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private float runSpeed = 2f;
    [SerializeField] Rigidbody2D m_Rigidbody2D;
    Vector3 m_Velocity = Vector3.zero;
    private float waitTime = 0f;
    [SerializeField] private bool facingRight = false;
    public bool autoRun = true;
    private float forceRunTime = 0f;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void FixedUpdate()
    {
        if ((autoRun && waitTime <= Time.time) || forceRunTime >= Time.time)
        {
            Vector3 targetVelocity;
            targetVelocity = new Vector2(runSpeed * Time.fixedDeltaTime * 10f, m_Rigidbody2D.velocity.y);
            if (!facingRight) targetVelocity.x *= -1;
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, 0.05f);
            // set facing direction
            FacePlayer();
        } else if (!autoRun)
        {
            // velocity goes to 0
            Vector3 targetVelocity;
            targetVelocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, 0.05f);
        }
    }

    public void DelayRun(float delayTime)
    {
        waitTime = Time.time + delayTime;
    }

    public void Run(float time)
    {
        forceRunTime = Time.time + time;
    }

    public void FacePlayer()
    {
        if (player.transform.position.x > transform.position.x ^ facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
