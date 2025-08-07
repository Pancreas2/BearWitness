using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BenchInteractable : Interactable
{
    [SerializeField] private Transform sitPosition;

    private PlayerMovement player;
    private GameManager gameManager;
    private Animator playerAnim;
    private BenchMenu benchMenu;

    private bool inUse = false;
    [SerializeField] private bool inArktis = false;
    private string benchName = "";

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        playerAnim = player.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        benchMenu = FindObjectOfType<BenchMenu>();

        benchName = SceneManager.GetActiveScene().name;
    }

    public override void OnInteract()
    {
        if (!inUse)
        {
            inUse = true;
            GameUI_Controller.instance.HideAll();
            benchMenu.SetOpen(true);
            player.WalkToPoint(sitPosition.position.x);
            playerAnim.SetBool("sitting", true);
            player.Freeze("bench");
            gameManager.pauseGameTime = true;

            if (!inArktis)
            {
                gameManager.StartSegmentRecording(benchName);  // only starts if this is the first bench; useful for testing
                gameManager.AddPOIToSegment(benchName);
            }

            benchMenu.Load();
        }
    }

    public void Exit()
    {
        GameUI_Controller.instance.ShowAll();
        player.ClearInputs();
        player.GetComponent<PlayerController>().CheckLantern();
        player.Unfreeze("bench");
        gameManager.pauseGameTime = false;
        playerAnim.SetBool("sitting", false);
        inUse = false;
    }
}
