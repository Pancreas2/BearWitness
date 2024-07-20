using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelPlayer : MonoBehaviour
{
    private PlayerController player;
    public bool active = true;

    [SerializeField] Vector2 repelForce;

    public Collider2D collider;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (active && collision.collider.CompareTag("Player") && collision.otherCollider.Equals(collider))
        {
            Debug.Log("repelling");
            Vector2 transmittedForce = new(repelForce.x * Mathf.Sign(player.transform.position.x - transform.position.x), repelForce.y);
            player.m_Rigidbody2D.AddForce(transmittedForce);
        }
    }

    public void SetActive(bool value)
    {
        active = value;
    }
}
