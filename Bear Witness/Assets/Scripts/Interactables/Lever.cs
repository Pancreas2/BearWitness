using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : ReceiveDamage
{
    private GameManager gameManager;
    [SerializeField] private bool oneUse;
    [SerializeField] private Gate.Gates doorName;
    [SerializeField] private Animator animator;
    private bool state;
    private int doorID;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (doorName != Gate.Gates.None)
        {
            doorID = Gate.GateMatch[doorName];
            state = gameManager.doorStates[doorID];
        }

        animator.SetBool("active", state);
    }

    public override void Damage(int damage = 0, float sourceX = 0f)
    {
        if (!state)
        {
            state = true;
            if (doorName != Gate.Gates.None)
                gameManager.doorStates[doorID] = true;
            animator.SetBool("active", true);
            Debug.Log(true);
        } else if (!oneUse)
        {
            state = false;
            if (doorName != Gate.Gates.None)
                gameManager.doorStates[doorID] = false;
            animator.SetBool("active", false);
        }
    }
}
