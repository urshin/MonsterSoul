using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SandWormFSM_Idle : StateMachineBehaviour
{
    [SerializeField] Transform UnderSetPosition;
    [SerializeField] Transform SetPosition;

    private float startTime;
    private float UnderSetPositionjourneyLength;
    private bool isMoving_UnderSetPosition = false;
    private float SetPositionjourneyLength;
    private bool isMoving_SetPosition = false;

    public float UndermovementSpeed = 200.0f; // 이동 속도
    public float movementSpeed = 2.0f; // 이동 속도

    [SerializeField] float timer;
    [SerializeField] float WaitingTime;

    // OnStateEnter는 전환 시작 시 호출되며 상태 기계가 이 상태를 평가하기 시작할 때입니다.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetPosition = GameObject.FindGameObjectWithTag("EnemyUpPos").transform;
        UnderSetPosition = GameObject.FindGameObjectWithTag("EnemyUnderPos").transform;

        animator.SetBool("AttackTime", false);
        if (animator.GetBool("Idle"))
        {

            // 현재 위치와 SetPosition 사이의 거리를 계산합니다.
            UnderSetPositionjourneyLength = Vector3.Distance(animator.transform.position, UnderSetPosition.position);
            startTime = Time.time;
            isMoving_UnderSetPosition = true;

            SetPositionjourneyLength = Vector3.Distance(UnderSetPosition.position, SetPosition.position);
            startTime = Time.time;
            isMoving_SetPosition = false;


            timer = 0;

         

        }
    }

    // OnStateUpdate는 OnStateEnter와 OnStateExit 콜백 사이의 각 업데이트 프레임에서 호출됩니다.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isMoving_UnderSetPosition)
        {

            // 현재 시간에 의해 이동한 거리를 계산합니다.
            float distanceCovered = (Time.time - startTime) * UndermovementSpeed;

            // 완료된 여정의 분수를 계산합니다.
            float fractionOfJourney = distanceCovered / UnderSetPositionjourneyLength;

            // 애니메이터의 위치를 SetPosition으로 향해 이동합니다.
            animator.transform.position = Vector3.Lerp(animator.transform.position, UnderSetPosition.position, fractionOfJourney);

            // 여정이 완료되었는지 확인합니다.
            if (fractionOfJourney >= 2.0f)
            {
                isMoving_UnderSetPosition = false;
                isMoving_SetPosition = true;
         
            }
        }

        if (isMoving_SetPosition)
        {

            // 현재 시간에 의해 이동한 거리를 계산합니다.
            float distanceCovered = (Time.time - startTime) * movementSpeed;

            // 완료된 여정의 분수를 계산합니다.
            float fractionOfJourney = distanceCovered / SetPositionjourneyLength;

            // 애니메이터의 위치를 SetPosition으로 향해 이동합니다.
            animator.transform.position = Vector3.Lerp(animator.transform.position, SetPosition.position, fractionOfJourney);

            // 여정이 완료되었는지 확인합니다.
            if (fractionOfJourney >= 2.0f)
            {
                isMoving_SetPosition = false;

            }
        }




        if (!isMoving_UnderSetPosition && !isMoving_SetPosition)
        {
            timer += Time.deltaTime;

            if (timer >= WaitingTime)
            {

                if(animator.GetFloat("HP") >= 30)
                {
                animator.SetBool("AttackTime",true);

                }
                if (animator.GetFloat("HP") < 30)
                {
                    // 30미만일때

                }

                //나중에 랜덤 함수 돌리기
                animator.SetBool("SWAttack1",true);
                animator.SetBool("Idle", false);
                timer= 0;
            }
        
        }


    }

    // OnStateExit는 전환 종료 시 호출되며 상태 기계가 이 상태를 평가를 완료할 때입니다.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("Idle", false);
    }

   
}