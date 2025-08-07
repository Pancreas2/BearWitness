using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D m_Rigidbody2D;
    [SerializeField] Animator animator;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private GameObject phaserBlast;

    private Vector3 velRef = Vector3.zero;
    private float m_MovementSmoothing = 0.05f;
    public bool jump = false;
    public bool run = false;

    public float facingDirection = 1;

    public void Jump(float force = 400f)
    {
        Vector2 vecForce = new (force / 2f * facingDirection, force);
        m_Rigidbody2D.AddForce(vecForce);
    }

    public void DiagonalJump(float force = 225f)
    {
        Vector2 vecForce = new(force * facingDirection, force);
        m_Rigidbody2D.AddForce(vecForce);
    }

    public void Move(float hmove)
    {
        Vector3 targetVelocity = new(hmove, m_Rigidbody2D.velocity.y, 0);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velRef, m_MovementSmoothing);
    }

    public void Flip()
    {
        transform.localScale = new Vector3(-facingDirection, 1);
        facingDirection = transform.localScale.x;
    }

    public void StopRun()
    {
        run = false;
        Vector3 decayVel = new(m_Rigidbody2D.velocity.x * 0.5f, m_Rigidbody2D.velocity.y); // much larger decay immediately
        m_Rigidbody2D.velocity = decayVel;
    }

    public void StartRun()
    {
        run = true;
    }

    private void InitializeTo(Vector3 position, Vector3 velocity)
    {
        transform.localPosition = position;
        m_Rigidbody2D.velocity = velocity;
    }

    public void InitializeSequence(int id)
    {
        switch (id)
        {
            case 0:
                InitializeTo(new (-1f, -0.03125f, 0f), Vector3.zero);
                break;

            case 1:
                InitializeTo(new(-9.12417f, -1.016263f, 0f), Vector3.zero);
                break;

            case 2:
                InitializeTo(new(-18.57738f, -0.5162745f, 0f), Vector3.zero);
                break;

            case 3:
                InitializeTo(new(-30.08776f, -0.5162238f, 0f), Vector3.zero);
                break;

            case 4:
                InitializeTo(new(-23.14835f, 2.733722f, 0f), Vector3.zero);
                break;

            case 5:
                InitializeTo(new(-26.375f, 8.71875f, 0f), Vector3.zero);
                break;

            case 6:
                InitializeTo(new(-25.12684f, 13.23373f, 0f), Vector3.zero);
                break;

            case 7:
                InitializeTo(new(-0.88f, 13.23377f, 0f), Vector3.zero);
                break;
        }
    }

    public void ApplyPhaserRecoil()
    {
        Vector3 vecForce = new(-800f * facingDirection, 100f);
        m_Rigidbody2D.AddForce(vecForce);

        GameObject phaserProjectile = Instantiate(phaserBlast);
        phaserProjectile.transform.position = transform.position;
        Vector2 ppVel = new(transform.localScale.x * 20f, 0f);
        phaserProjectile.GetComponent<Rigidbody2D>().velocity = ppVel;
    }

    public void Shoot()
    {
        StopRun();
        animator.Play("shoot", 1);
    }

    private void Update()
    {
        facingDirection = transform.localScale.x;

        if (jump)
        {
            Jump();
            jump = false;
        }

        if (run)
        {
            Move(facingDirection * movementSpeed);
        } else
        {
            Vector3 decayVel = new (m_Rigidbody2D.velocity.x * 0.925f, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, decayVel, ref velRef, m_MovementSmoothing);
        }

        animator.SetInteger("xSpeed", Mathf.RoundToInt(Mathf.Abs(m_Rigidbody2D.velocity.x)));
        animator.SetFloat("ySpeed", m_Rigidbody2D.velocity.y);
    }
}
