using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalixBoss : MonoBehaviour
{
    [SerializeField] private BaseEnemy baseEnemy;
    [SerializeField] private RunningEnemy runningEnemy;
    [SerializeField] private ContactDamage contactDamage;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbody;

    private string lastState = "idle";

    private float randomNum;
    private float attackSelectTime;

    private PlayerController player;

    [SerializeField] private Transform groundCheck;
    [SerializeField] LayerMask groundLayers;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();        
    }

    private void FixedUpdate()
    {
        AnimatorStateInfo currentAnimationState = animator.GetCurrentAnimatorStateInfo(1);
        if (currentAnimationState.IsName("walk"))
        {
            lastState = "walk";
            runningEnemy.autoRun = true;
        } else
        {
            if (currentAnimationState.IsName("jump"))
            {
                if (lastState != "jump")
                {
                    rigidbody.AddForce(new Vector2(140f * transform.localScale.x, 350f));
                    lastState = "jump";
                    runningEnemy.autoRun = true;
                }
            } else if (currentAnimationState.IsName("infightidle") || currentAnimationState.IsName("atk") || currentAnimationState.IsName("airatk"))
            {
                runningEnemy.autoRun = false;
                if (currentAnimationState.IsName("infightidle")) lastState = "idle";
                else lastState = "attack";
            }
            animator.SetFloat("ySpeed", rigidbody.velocity.y);

        }

        if (Mathf.Abs(player.transform.position.y - transform.position.y) < 2f && Mathf.Abs(player.transform.position.x - transform.position.x) < 1f)
        {
            if (lastState != "atk") // add smthing here
            {
                lastState = "atk";
                runningEnemy.FacePlayer();
            }
            animator.SetBool("playerInRange", true);
        } else
        {
            animator.SetBool("playerInRange", false);
        }

        if (attackSelectTime <= Time.time)
        {
            randomNum = Random.value;
            animator.SetFloat("Random", randomNum);
            attackSelectTime = Time.time + 0.5f;
        }

        bool onGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayers);
        animator.SetBool("grounded", onGround);
    }
}
