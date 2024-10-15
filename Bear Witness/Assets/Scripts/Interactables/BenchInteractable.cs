using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchInteractable : Interactable
{
    [SerializeField] private Transform sitPosition;

    private PlayerMovement player;
    private GameManager gameManager;
    private Animator playerAnim;
    private BenchMenu benchMenu;

    private bool inUse = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        playerAnim = player.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        benchMenu = FindObjectOfType<BenchMenu>();
    }

    public override void OnInteract()
    {
        if (!inUse)
        {
            inUse = true;
            benchMenu.Load();
            GameUI_Controller.instance.HideAll();
            benchMenu.SetOpen(true);
            player.WalkToPoint(sitPosition.position.x);
            playerAnim.SetBool("sitting", true);
            player.frozen = true;
            gameManager.pauseGameTime = true;
        }
    }

    public void Exit()
    {
        GameUI_Controller.instance.ShowAll();
        player.frozen = false;
        gameManager.pauseGameTime = false;
        playerAnim.SetBool("sitting", false);
        inUse = false;
    }
}
