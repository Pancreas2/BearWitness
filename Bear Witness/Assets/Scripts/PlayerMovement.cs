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
    float attackRate = 4f;
    float attackDelay = 0;
    public float moveTime = 0f;
    private Animator animator;

    public void DialogueMove(float distance)
    {
        float hMove = Mathf.Abs(distance);
        float time = distance / moveSpeed;
        horizontalMove = hMove * moveSpeed;
        moveTime = Time.time + time;
    }

    public void Perish()
    {
        Debug.Log("Perished");
    }

    private void Start()
    {
        animator = controller.GetComponent<Animator>();
    }
    void Update()
    {
        if (!frozen)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;

            if (Input.GetButtonDown("Jump"))
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
                if (Input.GetButtonDown("Special"))
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

            if (Input.GetButtonUp("Special")) animator.SetTrigger("attackEnd");

            wasFrozen = false;
        }
        else if (!wasFrozen)
        {
            wasFrozen = true;
            horizontalMove = 0;
            run = false;
            roll = false;
            attacking = false;
            jump = false;
        }
        else
        {
            if (moveTime <= Time.time)
            {
                horizontalMove = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (controller.inWater)
        {
            controller.Swim(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, jump);
        } else
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump, run, roll);
        }
        jump = false;
        if (attacking) controller.Attack(special);
        attacking = false;
    }
}
