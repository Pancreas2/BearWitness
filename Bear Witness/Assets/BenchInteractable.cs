using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchInteractable : Interactable
{
    [SerializeField] private Transform sitPosition;

    private PlayerMovement player;
    private GameManager gameManager;
    private Animator playerAnim;

    private bool inUse;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        playerAnim = player.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void OnInteract()
    {
        player.WalkToPoint(sitPosition.position.x);
        playerAnim.SetBool("sitting", true);
        player.frozen = true;
        gameManager.pauseGameTime = true;
    }
}
