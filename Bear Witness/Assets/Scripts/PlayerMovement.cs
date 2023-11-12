using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController controller;

    float horizontalMove = 0f;
    float verticalMove = 0f;
    public float moveSpeed = 10f;
    public float runMultiplier = 2f;
    bool jump = false;
    bool run = false;
    bool roll = false;
    bool special = false;
    public bool frozen = false;
    bool wasFrozen = false;
    bool attacking = false;
    readonly float attackRate = 3f;
    float attackDelay = 0;
    bool attackEnd = false;


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

            if (Input.GetButtonDown("Jump") && !wasFrozen)
            {
                jump = true;
            }

            if (controller.inWater)
            {
                verticalMove = Input.GetAxisRaw("Vertical") * moveSpeed;
            }
            else
            {

                if (Input.GetButton("Run"))
                {
                    run = true;
                    roll = false;
                    horizontalMove *= runMultiplier;
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
                else if (Input.GetMouseButton(0))
                {
                    special = false;
                    attacking = true;
                    roll = false;
                    run = false;
                    attackDelay = Time.time + (1f / attackRate);
                }
            }

            if (Input.GetButtonUp("Special")) {
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
            }
            jump = false;
            if (attacking) controller.Attack(special);
            attacking = false;
            if (attackEnd) controller.AttackEnd();
            attackEnd = false;
        }
        else {
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
                } else
                {
                    controller.Move(direction * moveSpeed * Time.fixedDeltaTime, false, false, false);
                }
            } else
            {
                controller.Move(0, false, false, false);
            }
        }
    }
}
