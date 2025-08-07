using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    public bool jumpContinuously = false;
    public float jumpForce = 300f;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded = false;
    public bool midairJumpAllowed = false;

    public void Jump()
    {
        if (grounded || midairJumpAllowed)
        {
            rigidbody.AddForce(new(0, jumpForce));
            grounded = false;
        }
    }

    private void Update()
    {
        if (jumpContinuously && grounded)
        {
            animator.SetTrigger("Jump");
        }

        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
            }
        }

        if (!grounded)
        {
            animator.ResetTrigger("Jump");
        }
    }
}
