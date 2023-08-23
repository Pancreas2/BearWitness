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

    // Update is called once per frame
    void Update()
    {
        
    }
}
