using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController controller;

    float horizontalMove = 0f;
    float verticalMove = 0f;
    public float moveSpeed = 15f;
    public float runMultiplier = 1.5f;
    bool jump = false;
    float jumpTime = 0f;
    bool run = false;
    bool roll = false;
    bool special = false;
    public bool frozen = false;
    bool wasFrozen = false;
    bool attacking = false;
    readonly float attackRate = 3.5f;
    float attackDelay = 0;
    bool attackEnd = false;

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
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;

        // was Input.GetButtonDown("Jump") && !wasFrozen
        if (Input.GetButtonDown("Jump") && !(onPassThroughPlatform && Input.GetAxisRaw("Vertical") < 0))
        {
            jump = true;
        }

        if (controller.inWater)
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
            if (Input.GetButton("Special"))
            {
                special = true;
                attacking = true;
                roll = false;
                run = false;
                attackDelay = Time.time + (2f / attackRate);
            }
            else if (Input.GetButton("Attack"))
            {
                special = false;
                attacking = true;
                roll = false;
                run = false;
                attackDelay = Time.time + (1f / attackRate);
            }
        }

        if (Input.GetButtonUp("Special"))
        {
            attackEnd = true;
        }

        wasFrozen = false;
    }

    private void FixedUpdate()
    {
        if (!frozen)
        {
            if (controller.inWater)
            {
                controller.Swim(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, jump);
            }
            else
            {
                controller.Move(horizontalMove * Time.fixedDeltaTime, jump, run, roll);
                if (attacking) controller.Attack(special);
                if (attackEnd) controller.AttackEnd();
            }
            jump = false;
            attacking = false;
            attackEnd = false;
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
                    transform.position.Set(moveTarget, transform.position.y, transform.position.z);
                    cutsceneMove = false;
                    if (cutsceneFaceRight ^ difference > 0f)
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
}
