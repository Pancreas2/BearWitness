using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalrusBoss : MonoBehaviour
{
    public BaseEnemy baseEnemy;

    private int attackType;

    public Collider2D collider;
    public Rigidbody2D m_Rigidbody;
    public float runSpeed;

    public Collider2D arenaBounds;
    public ContactFilter2D contactFilter;

    private float telegraphTime;

    private Vector3 nullVector = Vector3.zero;

    private void FixedUpdate()
    {
        bool telegraphing = telegraphTime > Time.time;
        switch (attackType)
        {
            case 0:
                // idle
                if (!telegraphing) SelectRandomAttack();
                break;

            case 1:
                // charging
                if (telegraphing)
                {

                } else
                {
                    baseEnemy.animator.Play(Animator.StringToHash("run"), 0);
                    List<Collider2D> walrusInArea = new(10);
                    arenaBounds.OverlapCollider(contactFilter, walrusInArea);
                    if (!walrusInArea.Contains(collider)) Rest();

                    m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, new Vector2(runSpeed, m_Rigidbody.velocity.y), ref nullVector, 0.05f);
                }

                break;
        }
    }

    public void RushAttack()
    {
        attackType = 1;
        telegraphTime = Time.time + 1f;
    }

    public void Rest()
    {
        attackType = 0;
        telegraphTime = Time.time + 1f;
    }

    public void SelectRandomAttack()
    {
        float random = Random.value;
        random -= 0.25f;
        if (random < 0f)
        {
            RushAttack();
            return;
        }
    }
}
