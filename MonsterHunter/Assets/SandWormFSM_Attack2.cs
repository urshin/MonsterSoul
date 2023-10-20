using AutoMoverPro;
using DistantLands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아래로 잠수, 플레이어 포지션으로 돌격


public class SandWormFSM_Attack2 : StateMachineBehaviour
{
    AutoMover wormAutoMover;
    GameObject Player;
    [SerializeField] Transform UnderSetPosition;
    [SerializeField] Transform SetRotation;
    Vector3 UnderPlayer;
    Vector3 UpPlayer;
    Vector3 Startpos;

    float PatternTime;


    float timer;
    // OnStateEnter는 전환 시작시 호출되며 상태 기계가 이 상태를 평가하기 시작합니다.

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PatternTime = 5f;
        timer = 0f;
        UnderPlayer = Player.transform.position + new Vector3(0, -50, 0);
        UpPlayer = Player.transform.position + new Vector3(0, 50, 0);
        Startpos = animator.transform.position + new Vector3(0, 20, 0);
        SandWormBoss.Instance.IsAttacking = true;
       SandWormBoss.Instance.MakingSinMovement(animator.gameObject, 40, PatternTime, Startpos, UnderPlayer, UpPlayer,SandWormBoss.Instance. UnderSetPosition.position+new Vector3(0,-30,0),0,"", AutoMoverLoopingStyle.loop);
        wormAutoMover = animator.GetComponent<AutoMover>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
       timer += Time.deltaTime;
        if(timer>= PatternTime)
        {
            if (wormAutoMover != null)
            {

                Destroy(wormAutoMover);
            }
            SandWormBoss.Instance.IsAttacking = false;
            SandWormBoss.Instance.StartPattern("Idle");

            SandWormBoss.Instance.StopPattern("SWAttack2");
        }
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
