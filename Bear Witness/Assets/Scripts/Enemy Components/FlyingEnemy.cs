using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlyingEnemy : MonoBehaviour
{

    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    [SerializeField] private Animator animator;
    private PlayerController player;
    [SerializeField] private bool doSwoopAttack = true;
    private bool attacking = false;
    private float attackTime = 0f;
    [SerializeField] private float minDistance = 1.5f;
    [SerializeField] private float maxDistance = 2.5f;
    [SerializeField] private bool facingRight = false;

    public UnityEvent OnSwoopAttack;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentPos = transform.position;
        Vector3 playerPos = player.transform.position;
        Vector3 posDifference = playerPos - currentPos;
        float distance = posDifference.magnitude;
        if (!doSwoopAttack || !attacking)
        {
            if (distance < 1.5f)
            {
                // move away
                m_Rigidbody2D.AddForce(-posDifference * 2f);
            }
            else if (distance > 2.5f)
            {
                // approach
                m_Rigidbody2D.AddForce(posDifference * 1.5f);
                attackTime = Mathf.Max(Random.Range(3f, 5f), attackTime);
            }
            else
            {
                // slow down
                m_Rigidbody2D.velocity /= 1.05f;
                RandomKick();
                if (attackTime <= Time.time)
                {
                    attacking = true;
                }
            }
        }
        else
        {
            if (distance < 1f && attacking)
            {
                // start attack
                OnSwoopAttack.Invoke();
                animator.SetTrigger("attack");
                m_Rigidbody2D.velocity /= 1.05f;
                attackTime = Time.time + Random.Range(3f, 5f);
                attacking = false;
            }
            else
            {
                // approach
                m_Rigidbody2D.AddForce(posDifference * 1.5f);
            }
        }
        if (posDifference.x > 0 ^ facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        animator.SetFloat("xSpeed", Mathf.Floor(m_Rigidbody2D.velocity.x) * transform.localScale.x);
    }

    private void RandomKick()
    {
        m_Rigidbody2D.AddForce(new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f)));
    }

    public void ResetAttack()
    {
        attackTime = Time.time + Random.Range(3f, 5f);
        attacking = false;
    }
}
