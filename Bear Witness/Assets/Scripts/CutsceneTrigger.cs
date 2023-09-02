using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneTrigger : MonoBehaviour
{
    public bool triggerOnFirstLoad;
    public bool triggerOnColliderEnter;
    public Collider2D collider;
    public string cutscene_ID;
    private GameManager gameManager;

    public UnityEvent OnCutsceneStart;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (triggerOnFirstLoad && !gameManager.playedCutscenes.Contains(cutscene_ID))
        {
            OnCutsceneStart.Invoke();
            gameManager.playedCutscenes.Add(cutscene_ID);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggerOnColliderEnter && collision.collider.CompareTag("Player"))
        {
            if (!gameManager.playedCutscenes.Contains(cutscene_ID))
            {
                OnCutsceneStart.Invoke();
                gameManager.playedCutscenes.Add(cutscene_ID);
            }
        }
    }
}
