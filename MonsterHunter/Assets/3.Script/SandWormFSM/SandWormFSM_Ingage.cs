using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormFSM_Ingage : StateMachineBehaviour
{
    public float moveSpeed = 200f; // �̵� �ӵ�
    private Transform player; // �÷��̾��� ��ġ�� ������ ����
    private Vector3 direction;
    private float initialDistance; // �ʱ� �÷��̾���� �Ÿ�

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Idle", false);
        // "Player" �±׸� ���� ������Ʈ�� ã�Ƽ� player ������ �Ҵ�
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
            // ������ ���� �̵�
            animator.transform.position += (direction * moveSpeed) * Time.deltaTime;
            // ���� ��ġ�� �÷��̾��� ��ġ ������ �Ÿ��� ���
            float distance = Vector3.Distance(animator.transform.position, player.position);

            // ���� �ʱ� �Ÿ����� Ŀ���� Ingage ���¸� ����
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
