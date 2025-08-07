using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barnacle : MonoBehaviour
{
    [SerializeField] BaseEnemy baseEnemy;
    [SerializeField] GameObject projectile;
    [SerializeField] private float cooldownDuration = 2f;
    float cooldown = 0f;

    private void FixedUpdate()
    {
        cooldown -= Time.fixedDeltaTime;
        baseEnemy.animator.SetFloat("cooldown", cooldown);
    }

    public void ShootBullet()
    {
        cooldown = cooldownDuration;
        GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
        PlayerController player = FindObjectOfType<PlayerController>();
        Vector3 velocity = Vector3.Normalize(player.transform.position - transform.position);
        velocity *= 2f;
        bullet.GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
