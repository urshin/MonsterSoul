using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormFSM_Ingage : StateMachineBehaviour
{
    public float moveSpeed = 200f; // 이동 속도
    private Transform player; // 플레이어의 위치를 저장할 변수
    private Vector3 direction;
    private float initialDistance; // 초기 플레이어와의 거리

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Idle", false);
        // "Player" 태그를 가진 오브젝트를 찾아서 player 변수에 할당
        player = GameObject.FindGameObjectWithTag("Player").transform;
        direction = (player.position - animator.transform.position).normalized;
        initialDistance = Vector3.Distance(animator.transform.position, player.position);
        SandWormBoss.Instance.IsAttacking = true;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        if (player != null)
        {
            // 방향을 따라 이동
            animator.transform.position += (direction * moveSpeed) * Time.deltaTime;
            // 현재 위치와 플레이어의 위치 사이의 거리를 계산
            float distance = Vector3.Distance(animator.transform.position, player.position);

            // 만약 초기 거리보다 커지면 Ingage 상태를 종료
            if (distance > initialDistance)
            {
                SandWormBoss.Instance.IsAttacking = false;
                animator.SetBool("Ingage", false);
               animator.SetBool("Idle", true);

            }
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
