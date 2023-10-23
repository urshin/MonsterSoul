using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class SandWormFSM_Idle : StateMachineBehaviour
{


    bool isUnderSetPosition;
    bool isSetPosition;

    [SerializeField] float timer;
    [SerializeField] float WaitingTime;



    // OnStateEnter는 전환 시작 시 호출되며 상태 기계가 이 상태를 평가하기 시작할 때입니다.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        SandWormBoss.Instance.SandWormLastPosition = animator.transform;
        animator.SetBool("AttackTime", false);



        if (animator.GetBool("Idle"))
        {

            SoundManager.Instance.PlayEffect("UnderRoar");
            animator.gameObject.transform.position = SandWormBoss.Instance.UnderSetPosition.position;
            SandWormBoss.Instance.Goto(animator.gameObject, SandWormBoss.Instance.SandWormLastPosition.position, SandWormBoss.Instance.SetPosition.position, 10f);
            SandWormBoss.Instance.IsAttacking= false;


            timer = 0;
        }
    }

    // OnStateUpdate는 OnStateEnter와 OnStateExit 콜백 사이의 각 업데이트 프레임에서 호출됩니다.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {





        timer += Time.deltaTime;

        if (timer >= WaitingTime)
        {

            if (animator.GetFloat("HP") >= 30)
            {
                animator.SetBool("AttackTime", true);

            }
            if (animator.GetFloat("HP") < 30)
            {
                // 30미만일때

            }

            //나중에 랜덤 함수 돌리기
            int randomPattern = Random.Range(0, 3); 
            switch (randomPattern)
            {
                case 0:
                    animator.SetBool("SWAttack1", true);
                    break;
                case 1:
                    animator.SetBool("SWAttack2", true);
                    break;
                case 2:
                    animator.SetBool("SWAttack3", true);
                    break;
            }


           
            animator.SetBool("Idle", false);
            timer = 0;
        }




    }

    // OnStateExit는 전환 종료 시 호출되며 상태 기계가 이 상태를 평가를 완료할 때입니다.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("Idle", false);
    }


}