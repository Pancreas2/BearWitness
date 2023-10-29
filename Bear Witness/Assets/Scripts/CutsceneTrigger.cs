using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneTrigger : MonoBehaviour
{
    public bool reuseable = false;
    public bool triggerOnFirstLoad;
    public bool triggerOnColliderEnter;
    public Collider2D collider;
    public string cutscene_ID;
    private GameManager gameManager;
    public float delay;
    private float cutsceneStartTime = -5f;
    private bool used = false;

    public UnityEvent OnCutsceneStart;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (triggerOnFirstLoad && reuseable || !gameManager.playedCutscenes.Contains(cutscene_ID))
        {
            cutsceneStartTime = Time.time + delay;
        }
    }

    private void Update()
    {
        if (!used && cutsceneStartTime >= 0 && Time.time > cutsceneStartTime)
        {
            TriggerCutscene();
            used = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggerOnColliderEnter && collision.collider.CompareTag("Player") && reuseable || !gameManager.playedCutscenes.Contains(cutscene_ID))
        {
            cutsceneStartTime = Time.time + delay;
        }
    }

    private void TriggerCutscene()
    {
        OnCutsceneStart.Invoke();
        if (!reuseable)
            gameManager.playedCutscenes.Add(cutscene_ID);
    }
}
