using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimTrigger : StateMachineBehaviour
{ 
    public string parameter;

public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{
    base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetTrigger(parameter);
}
// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{

}

// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{

}

public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{

}

public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{

}
}
