using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarlottaBoss : MonoBehaviour
{
    [SerializeField] private GameObject carlottaFeather;
    [SerializeField] private Transform featherLaunchPoint;

    [SerializeField] private BossEnemy bossEnemy;

    private float attackTime;
    private float runDelayTime;

    private PlayerController player;
    Vector3 visibleDirection;
    private float distanceToPlayer;

    private float runDirection = 0f;
    public bool m_FacingRight = false;

    private Vector3 refVel = Vector3.zero;

    private bool chargeMelee = false;
    private int timesRun = 0;

    private float hurtTime = 0f;
    private int comboCount = 0;

    private bool inFight = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();    
    }

    public void FeatherLaunch()
    {
        float facingDirection = transform.localScale.x;

        for (int i = -1; i < 2; i++)
        {
            float angle = 35f * i;
            Vector2 force = new(facingDirection * 60f * Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad) * 60f);
            GameObject feather = Instantiate(carlottaFeather, featherLaunchPoint.position, Quaternion.Euler(0f, 0f, facingDirection * angle));
            feather.transform.localScale = new Vector3(facingDirection, 1f, 1f);
            feather.GetComponent<Rigidbody2D>().AddForce(force);
        }
    }

    public void FlapUp()
    {
        Debug.Log("flap");
        Vector2 flapForce = new(0f, 150f);
        bossEnemy.m_Rigidbody2D.AddForce(flapForce);
    }

    public void FixedUpdate()
    {
        if (inFight)
        {
            PhaseOneLoop();

            if (hurtTime <= Time.time)
            {
                comboCount = 0;
            }
        }
    }

    private void PhaseOneLoop()
    {
        if (attackTime <= Time.time)
        {
            chargeMelee = false;
            runDirection = 0f;
            distanceToPlayer = (player.transform.position - transform.position).magnitude;
            float random = Random.value;
            if (distanceToPlayer < 0.7f && random < 0.8f)
            {
                bossEnemy.animator.Play("carlotta_double_slash");
                attackTime = Time.time + 1f;
            }
            else
            {
                random = Random.value;
                if (random < 0.2f || (timesRun >= 3 && random < 0.4f))
                {
                    FacePlayer();
                    bossEnemy.animator.Play("carlotta_fling");
                    attackTime = Time.time + 2f;
                    timesRun = 0;
                }
                else if (random < 0.55f && timesRun < 3)
                {
                    FacePlayer();
                    runDirection = Mathf.Sign(player.transform.position.x - transform.position.x);
                    if (random < 0.3f && distanceToPlayer < 1.5f) runDirection *= -1;
                    bossEnemy.animator.Play("carlotta_run");
                    attackTime = Time.time + 1f;
                    timesRun++;
                }
                else if (random < 0.7f && timesRun < 3)
                {
                    FacePlayer();
                    runDirection = Mathf.Sign(player.transform.position.x - transform.position.x);
                    bossEnemy.animator.Play("carlotta_run");
                    attackTime = Time.time + 1f;
                    chargeMelee = true;
                }
                else if (random < 0.9f || timesRun >= 3)
                {
                    if (timesRun >= 3) timesRun = 0;
                    FacePlayer();
                    bossEnemy.animator.Play("carlotta_dash");
                    runDelayTime = Time.time + 0.4f;
                    attackTime = Time.time + 1.8f;
                    runDirection = Mathf.Sign(player.transform.position.x - transform.position.x) * 3;
                } else
                {
                    attackTime = Time.time + 0.5f;
                }
            }

        }

        if (chargeMelee)
        {
            distanceToPlayer = (player.transform.position - transform.position).magnitude;
            if (distanceToPlayer < 0.7f)
            {
                bossEnemy.animator.Play("carlotta_double_slash");
                chargeMelee = false;
                FacePlayer();
                runDirection = -Mathf.Sign(player.transform.position.x - transform.position.x);
                runDelayTime = Time.time + 1f;
                attackTime = Time.time + 2f;
                timesRun++;
            }
        }

        if (runDelayTime <= Time.time)
        {
            Vector3 targetVel = new(runDirection * 3f, bossEnemy.m_Rigidbody2D.velocity.y, 0f);
            bossEnemy.animator.SetFloat("xSpeed", bossEnemy.m_Rigidbody2D.velocity.x);

            bossEnemy.m_Rigidbody2D.velocity = Vector3.SmoothDamp(bossEnemy.m_Rigidbody2D.velocity, targetVel, ref refVel, 0.05f);
        }
    }

    public void FacePlayer()
    {
        if (player.transform.position.x - transform.position.x > 0 ^ m_FacingRight)
        {
            // flip
            m_FacingRight = !m_FacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    public void CarlottaHurt()
    {
        if (!bossEnemy.animator.GetCurrentAnimatorStateInfo(0).IsName("carlotta_flap_push"))
        {
            hurtTime = Time.time + 0.5f;
            attackTime = Mathf.Max(attackTime, Time.time + 1f);
            runDelayTime = Mathf.Max(attackTime, Time.time + 1f);
            runDirection = 0f;
            if (comboCount >= 3)
            {
                FacePlayer();
                bossEnemy.animator.Play("carlotta_flap_push");
                comboCount = 0;
                // force the next attack to be feather throw
                timesRun = 5;
                attackTime = Time.time + 0.5f;
            }
            else
            {
                bossEnemy.animator.Play("carlotta_flap");
                comboCount++;
            }
        }
    }

    public void StartFight()
    {
        if (!inFight)
        {
            inFight = true;
            attackTime = Time.time + 0.5f;
            bossEnemy.BeginFight();
        }
    }

    public void EndFight()
    {
        inFight = false;
        GameManager.instance.GrantAchievement("Flying Feathers");
        bossEnemy.animator.SetTrigger("MidFightDialogue");
    }
}
