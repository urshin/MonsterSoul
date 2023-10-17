using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SandWormFSM_Idle : StateMachineBehaviour
{
    public Transform UnderSetPosition;
    public Transform SetPosition;

    private float journeyLength;
    private float startTime;
    private bool isMoving = false;

    public float movementSpeed = 2.0f; // 이동 속도

    // OnStateEnter는 전환 시작 시 호출되며 상태 기계가 이 상태를 평가하기 시작할 때입니다.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("Idle"))
        {
            animator.transform.position = UnderSetPosition.position;
            // 현재 위치와 SetPosition 사이의 거리를 계산합니다.
            journeyLength = Vector3.Distance(animator.transform.position, SetPosition.position);
            startTime = Time.time;
            isMoving = true;
        }
    }

    // OnStateUpdate는 OnStateEnter와 OnStateExit 콜백 사이의 각 업데이트 프레임에서 호출됩니다.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isMoving)
        {
            // 현재 시간에 의해 이동한 거리를 계산합니다.
            float distanceCovered = (Time.time - startTime) * movementSpeed;

            // 완료된 여정의 분수를 계산합니다.
            float fractionOfJourney = distanceCovered / journeyLength;

            // 애니메이터의 위치를 SetPosition으로 향해 이동합니다.
            animator.transform.position = Vector3.Lerp(animator.transform.position, SetPosition.position, fractionOfJourney);

            // 여정이 완료되었는지 확인합니다.
            if (fractionOfJourney >= 1.0f)
            {
                isMoving = false;
                animator.SetBool("Idle", false);
                //나중에 시간 체크하고 공격 모션 움직이기



            }
        }
    }

    // OnStateExit는 전환 종료 시 호출되며 상태 기계가 이 상태를 평가를 완료할 때입니다.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Idle", false);
    }

    // OnStateMove는 Animator.OnAnimatorMove() 이후 바로 호출됩니다.
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // 루트 모션을 처리하고 영향을 미치는 코드를 구현합니다.
    //}

    // OnStateIK는 Animator.OnAnimatorIK() 이후 바로 호출됩니다.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // 애니메이션 IK (역행운학)을 설정하는 코드를 구현합니다.
    //}
}