using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_LockOnMovement : StateMachineBehaviour
{
    DefMovement movement;
    private bool isWalk1Playing = false;
    private float lastSoundTime = 0.0f;
    private float soundInterval = 0.5f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsAttack", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //float movement = animator.GetFloat("Movement");

        //// Check if it's time to play a sound
        //if (movement >= 0.1f || movement <= -0.1f)
        //{
        //    if (Time.time - lastSoundTime >= soundInterval)
        //    {
        //        if (isWalk1Playing)
        //        {
        //            SoundManager.Instance.PlayEffect("Walk2");
        //            isWalk1Playing = false;
        //        }
        //        else
        //        {
        //            SoundManager.Instance.PlayEffect("Walk1");
        //            isWalk1Playing = true;
        //        }

        //        lastSoundTime = Time.time;
        //    }
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
