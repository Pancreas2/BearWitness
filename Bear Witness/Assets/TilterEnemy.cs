using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilterEnemy : MonoBehaviour
{

    [SerializeField] private BaseEnemy baseEnemy;
    private Rigidbody2D m_Rigidbody2D;
    private Animator animator;
    private PlayerController player;
    private bool facingRight = false;
    private int state = 0;
    // idle, following, preparing attack, attacking
    private float attackTime = 0f;
    [SerializeField] private float noticeDistance = 5f;
    [SerializeField] private LayerMask raycastLayers;
    [SerializeField] private Transform eyePoint;

    private void Start()
    {
        m_Rigidbody2D = baseEnemy.m_Rigidbody2D;
        animator = baseEnemy.animator;
        player = FindObjectOfType<PlayerController>();
    }
}
