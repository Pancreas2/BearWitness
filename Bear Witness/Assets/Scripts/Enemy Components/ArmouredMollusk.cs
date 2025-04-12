using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmouredMollusk : MonoBehaviour
{
    [SerializeField] private BaseEnemy baseEnemy;
    [SerializeField] private Animator animator;
    [SerializeField] private LookForPlayer lookForPlayer;

    private PlayerController player;

    private float attackTime;
    [SerializeField] private bool facingRight = false;
    private float hideTime = 0f;
    private float xDifference;
    private float chargeTime;

    private bool charging;

    private float home;
    private bool homeFacesRight;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        home = transform.position.x;
        homeFacesRight = facingRight;
    }

    private void FixedUpdate()
    {
        if (lookForPlayer.seesPlayer)
        {
            xDifference = player.transform.position.x - transform.position.x;
            if (!charging && attackTime - 0.5f > Time.time)
                FacePlayer();
            if (attackTime <= Time.time && !charging)
            {
                if (Random.value < 0.2f && Mathf.Abs(xDifference) < 1f)
                {
                    animator.SetTrigger("hide");
                    ResetHideTime();
                } else
                {
                    if ((Mathf.Abs(xDifference) > 1f || Random.value < 0.3f) && (xDifference < 0 ^ facingRight))
                    {
                        charging = true;
                        animator.SetBool("charging", true);
                        chargeTime = Time.time + 2f;
                    }

                    animator.SetTrigger("swing");
                }

                attackTime = Time.time + 1.5f;
            }

            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (charging && currentState.IsName("am_charge_loop"))
            {
                if (xDifference < 0 ^ facingRight)
                {
                    Vector3 chargeSpeed = new(4f, 0f);
                    if (!facingRight) chargeSpeed *= -1f;
                    baseEnemy.m_Rigidbody2D.velocity = chargeSpeed;
                    attackTime = Time.time + 1.5f;
                }
            }

            if (chargeTime <= Time.time)
            {
                charging = false;
                animator.SetBool("charging", false);
            }

            if (hideTime <= Time.time)
            {
                if (currentState.IsName("am_hide"))
                    animator.SetTrigger("unhide");
                baseEnemy.invulnerable = false;
            }
            else
            {
                baseEnemy.invulnerable = true;
            }
        } else if (charging)
        {
            charging = false;
            animator.SetBool("charging", false);
        }
    }

    public void ResetHideTime()
    {
        hideTime = Time.time + 0.5f + Random.value;
    }

    private void FacePlayer()
    {
        if (xDifference > 0 ^ facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    public void ReturnToPost()
    {
        StartCoroutine(WalkingHome());
    }

    IEnumerator WalkingHome()
    {
        xDifference = home - transform.position.x;
        while (Mathf.Abs(xDifference) > 0.1f && !lookForPlayer.seesPlayer)
        {
            xDifference = home - transform.position.x;
            FacePlayer();
            Vector3 walkSpeed = new(1f, 0f);
            if (!facingRight) walkSpeed *= -1f;
            baseEnemy.m_Rigidbody2D.velocity = walkSpeed;
            yield return new WaitForFixedUpdate();
        }
        baseEnemy.m_Rigidbody2D.velocity = Vector3.zero;
        if (facingRight ^ homeFacesRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
