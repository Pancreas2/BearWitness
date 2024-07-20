using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController controller;
    [SerializeField] private Animator animator;

    float horizontalMove = 0f;
    float verticalMove = 0f;
    public float moveSpeed = 15f;
    public float runMultiplier = 1.5f;
    bool jump = false;
    float jumpTime = 0f;
    bool run = false;
    bool roll = false;
    public bool frozen = false;
    bool wasFrozen = false;
    public float freezeTime;
    bool attacking = false;
    int attackButton = 0;
    readonly float attackRate = 3.5f;
    float attackDelay = 0;
    bool attackEnd = false;
    public bool climbing = false;

    public bool onPassThroughPlatform = false;

    public float moveTarget = 0f;
    private bool cutsceneMove = false;
    public bool cutsceneFaceRight = false;

    public void WalkToPoint(float pointX)
    {
        cutsceneMove = true;
        wasFrozen = true;
        frozen = true;

        moveTarget = pointX;
    }

    public void Perish()
    {
        Debug.Log("Perished");
    }

    void Update()
    {
        if (!climbing)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        }

        // was Input.GetButtonDown("Jump") && !wasFrozen
        if (Input.GetButtonDown("Jump") && !(onPassThroughPlatform && Input.GetAxisRaw("Vertical") < 0))
        {
            jump = true;
            climbing = false;
        }

        if (controller.inWater || climbing)
        {
            verticalMove = Input.GetAxisRaw("Vertical") * moveSpeed;
            run = false;
            roll = false;
        }
        else
        {
            if (controller.m_Grounded)
            {
                if (Input.GetButton("Run") || Input.GetAxisRaw("RightTrigger") > 0.25f)
                {
                    run = true;
                    roll = false;
                }
                else if ((run || roll) && horizontalMove != 0)
                {
                    roll = true;
                }
                else
                {
                    run = false;
                    roll = false;
                }

                if (horizontalMove == 0) run = false;
            } else if (roll && horizontalMove == 0)
            {
                roll = false;
                run = false;
            }

            if (run)
            {
                if (roll)
                {
                    horizontalMove *= 1.25f;
                } else
                {
                    horizontalMove *= runMultiplier;
                }
            }
        }

        if (Time.time >= attackDelay)
        {
            if (Input.GetButtonDown("AttackB"))
            {
                attackButton = 2;
                attacking = true;
                roll = false;
                run = false;
                attackDelay = Time.time + (1f / attackRate);
            }
            else if (Input.GetButtonDown("AttackY"))
            {
                attackButton = 1;
                attacking = true;
                roll = false;
                run = false;
                attackDelay = Time.time + (1f / attackRate);
            } else if (Input.GetButtonDown("AttackX"))
            {
                attackButton = 0;
                attacking = true;
                roll = false;
                run = false;
                attackDelay = Time.time + (1f / attackRate);
            }
        }

        if ((Input.GetButtonUp("AttackB") && attackButton == 2) || (Input.GetButtonUp("AttackY") && attackButton == 1) || (Input.GetButtonUp("AttackX") && attackButton == 0))
        {
            Debug.Log("attackEnd");
            attackEnd = true;
        }
    }

    private void FixedUpdate()
    {
        animator.SetBool("climbing", climbing);
        if (!frozen)
        {
            if (controller.inWater)
            {
                controller.Swim(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, jump);
            }
            else if (climbing)
            {
                controller.Climb(verticalMove * Time.fixedDeltaTime);
            } else
            {
                controller.Move(horizontalMove * Time.fixedDeltaTime, jump, run, roll);
                if (attacking) controller.Attack(attackButton);
                if (attackEnd) controller.AttackEnd();
            }
            jump = false;
            attacking = false;
            attackEnd = false;

            wasFrozen = false;
        }
        else
        {
            if (!wasFrozen)
            {
                wasFrozen = true;
                horizontalMove = 0;
                run = false;
                roll = false;
                attacking = false;
                jump = false;
            }
            if (cutsceneMove)
            {
                float difference = moveTarget - transform.position.x;
                float direction = Mathf.Sign(difference);
                if (Mathf.Abs(difference) < moveSpeed * Time.fixedDeltaTime)
                {
                    transform.position = new(moveTarget, transform.position.y);
                    controller.m_Rigidbody2D.velocity = Vector3.zero;
                    cutsceneMove = false;
                    if (cutsceneFaceRight ^ transform.localScale.x == 1f)
                    {
                        controller.Flip();
                    }
                }
                else
                {
                    controller.Move(direction * moveSpeed * Time.fixedDeltaTime, false, false, false);
                }
            }
            else
            {
                controller.Move(0, false, false, false);
            }
        }
    }

    public void PlayAnimation(string name)
    {
        animator.Play(name, 5);
    }

}
