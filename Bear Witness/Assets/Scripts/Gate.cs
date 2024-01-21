using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public int gateID;
    private GameManager gameManager;
    [SerializeField] private Animator animator;
    [SerializeField] private bool forceOpen;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (gameManager.doorStates[gateID] || forceOpen)
        {
            animator.SetBool("open", true);
        } else
        {
            animator.SetBool("open", false);
        }
    }

    public void SetForceOpen(bool value)
    {
        forceOpen = value;
    }
}
