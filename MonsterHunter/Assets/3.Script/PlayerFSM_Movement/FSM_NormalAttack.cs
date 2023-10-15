using AutoMoverPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_NormalAttack : StateMachineBehaviour
{
    

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        

    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetBool("IsAttack", true);
        Debug.Log("공격상태11111111111");
        DefMovement.Instance.Nowspeed = 0;
        DefMovement.Instance.dir = DefMovement.Instance.transform.forward;
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Player.Instance.PlayerMotionDamage = 0;
        Debug.Log("공격아님222222222222");
        //movement.Nowspeed = movement.moveSpeed;
        animator.SetBool("IsAttack", false);
        animator.SetBool("AbleCombo", false);

    }
}
