using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : StateMachineBehaviour
{
    public bool stopSoundsOnEnter;
    public bool stopSoundsOnExit;
    public string soundName;
    public float startTime;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stopSoundsOnEnter) audioManager.StopAll();
        audioManager.Play(soundName, startTime);

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stopSoundsOnExit) audioManager.StopAll();
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
