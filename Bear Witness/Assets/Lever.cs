using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private bool oneUse;
    [SerializeField] private int doorID;
    [SerializeField] private Animator animator;
    private bool state;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        state = gameManager.doorStates[doorID];
        animator.SetBool("active", state);
    }

    public void FlickLever()
    {
        if (!state)
        {
            state = true;
            gameManager.doorStates[doorID] = true;
            animator.SetBool("active", true);
            Debug.Log(true);
        } else if (!oneUse)
        {
            state = false;
            gameManager.doorStates[doorID] = false;
            animator.SetBool("active", false);
        }
    }
}
