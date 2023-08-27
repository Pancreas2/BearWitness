using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController controller;
    float horizontalMove = 0f;
    public float moveSpeed = 10f;
    public float runMultiplier = 2f;
    bool jump = false;
    bool run = false;
    bool roll = false;
    bool special = false;
    public bool frozen = false;
    bool wasFrozen = false;
    bool attacking = false;
    float attackRate = 2f;
    float attackDelay = 0;
    public float moveTime = 0f;

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

    // Update is called once per frame
    void Update()
    {
        if (!frozen)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;

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

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }

            if (Time.time >= attackDelay)
            {
                if (Input.GetButtonDown("Special"))
                {
                    special = true;
                    attacking = true;
                    roll = false;
                    run = false;
                    attackDelay = Time.time + (1f / attackRate);
                } else if (Input.GetMouseButtonDown(0))
                {
                    special = false;
                    attacking = true;
                    roll = false;
                    run = false;
                    attackDelay = Time.time + (1f / attackRate);
                }
            }
            wasFrozen = false;
        } else if (!wasFrozen)
        {
            wasFrozen = true;
            horizontalMove = 0;
            run = false;
            roll = false;
            attacking = false;
            jump = false;
        } else
        {
            if (moveTime <= Time.time)
            {
                horizontalMove = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, run, roll);
        jump = false;
        if (attacking) controller.Attack(special);
        attacking = false;
    }
}
