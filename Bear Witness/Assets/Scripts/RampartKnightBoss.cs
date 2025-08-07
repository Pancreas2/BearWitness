using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampartKnightBoss : MonoBehaviour
{
    [SerializeField] private BossEnemy bossEnemy;
    [SerializeField] private Transform spearSpawnParent;
    [SerializeField] private List<Transform> spearSpawners;
    [SerializeField] private Transform handSpearSpawner;
    [SerializeField] private GameObject spear;
    [SerializeField] private GameObject shockwave;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private CinemachineShake arenaCamShake;

    private float attackTime;
    private int prevAttack = 0;
    private bool bounceOnLand = false;

    private bool facingRight = false;
    private float facingDirection = -1;

    private float spearSpawnerMoveTime;

    private void FixedUpdate()
    {
        if (bossEnemy.fightActive)
        {
            DecideAttacks();

            if (bounceOnLand)
            {
                BounceOnGround();
            }
        }
    }

    public void StartFight()
    {
        attackTime = Time.time + 1.25f;
        bossEnemy.BeginFight();
        bossEnemy.UpdateHealth();
        bossEnemy.fightActive = true;
    }

    public void EndFight()
    {
        GameManager.instance.GrantAchievement("Erosion");
        StopAllCoroutines();
    }

    private void BounceOnGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && bossEnemy.m_Rigidbody2D.velocity.y < 0)
            {
                bounceOnLand = false;
                bossEnemy.m_Rigidbody2D.velocity = Vector3.zero;
                Vector2 jumpForce = new(facingDirection * 80f, 160f);
                bossEnemy.m_Rigidbody2D.AddForce(jumpForce);
                CreateShockWave(groundCheck);
                arenaCamShake.ShakeCamera(0.25f, 1f);
                AudioManager.instance.Play("RKLand", fadeTime: 0);
                break;
            }
        }
    }

    private void DecideAttacks()
    {
        if (attackTime <= Time.time)
        {
            if (FindObjectOfType<PlayerController>().transform.position.x > transform.position.x ^ facingRight)
            {
                Flip();
                attackTime = Time.time + 0.5f;
                return;
            }

            Debug.Log("ATTACK!!");

            float rand = UnityEngine.Random.value;
            if (prevAttack == 4) rand /= 1.25f;

            if (rand < 0.2f && prevAttack > 2)
            {
                prevAttack = 1;

                if (spearSpawnerMoveTime <= Time.time)
                {
                    float randOffset = Mathf.FloorToInt(UnityEngine.Random.value * 7f) - 3;
                    randOffset /= 4f;
                    Vector3 parentPosition = new(randOffset, 0, 0);
                    spearSpawnParent.transform.localPosition = parentPosition;
                    spearSpawnerMoveTime = Time.time + 2f;
                }

                for (int i = 0; i < 7; i++)
                {
                    Instantiate(spear, spearSpawners[i]);
                }

                bossEnemy.animator.Play("rk_spear_spin", 1);
                attackTime = Time.time + 2f;
            } else if (rand <= 0.35f && prevAttack != 2)
            {
                prevAttack = 2;

                if (spearSpawnerMoveTime <= Time.time)
                {
                    float randOffset = Mathf.FloorToInt(UnityEngine.Random.value * 7f) - 3;
                    randOffset /= 4f;
                    Vector3 parentPosition = new(randOffset, 0, 0);
                    spearSpawnParent.transform.localPosition = parentPosition;
                    spearSpawnerMoveTime = Time.time + 8f;
                }

                for (int i = 0; i < 7; i++)
                {
                    StartCoroutine(DelaySpawnSpear(i * 0.5f, i));
                }

                for (int i = 0; i < 7; i++)
                {
                    StartCoroutine(DelaySpawnSpear(i * 0.5f + 4.5f, 6 - i));
                }

                bossEnemy.animator.Play("rk_spear_spin", 1);
                attackTime = Time.time + 2f;
            } else if (rand <= 0.5f)
            {
                prevAttack = 3;
                bossEnemy.animator.Play("rk_jump", 1);
                attackTime = Time.time + 3f;
            } else if (rand <= 0.9f)
            {
                prevAttack = 4;
                bossEnemy.animator.Play("rk_stab", 1);
                StartCoroutine(SpawnHSpear());
                attackTime = Time.time + 1.75f;
            } else
            {
                prevAttack = 0;
                attackTime = Time.time + 0.2f; // very small wait time
            }
        }
    }

    IEnumerator DelaySpawnSpear(float delay, int index)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(spear, spearSpawners[index]);
    }

    IEnumerator SpawnHSpear()
    {
        yield return new WaitForSeconds(0.1f);
        AudioSource source = Instantiate(spear, handSpearSpawner).GetComponent<AudioSource>();
        source.pitch = 1.5f;
        source.volume = 0.75f;
    }

    public void Jump()
    {
        Vector2 jumpForce = new(facingDirection * 100f, 400f);
        bossEnemy.m_Rigidbody2D.AddForce(jumpForce);
    }

    private void Flip()
    {
        Vector3 newScale = new(-transform.localScale.x, 1, 1);
        transform.localScale = newScale;
        facingRight = !facingRight;
        facingDirection = -transform.localScale.x;
    }

    public void QueueBounce()
    {
        if (transform.position.y > 1.5f)  // only bounce if you jumped high enough
            bounceOnLand = true;
    }

    private void CreateShockWave(Transform creationPoint)
    {
        Instantiate(shockwave, creationPoint.position + new Vector3(0.5f, 0f, 0f), creationPoint.rotation).GetComponent<Shockwave>().SetSpeed(2 + facingDirection);
        Instantiate(shockwave, creationPoint.position - new Vector3(0.5f, 0f, 0f), creationPoint.rotation).GetComponent<Shockwave>().SetSpeed(-2 + facingDirection);
    }
}
