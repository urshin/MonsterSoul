//옆으로 헤엄지면서 공격하는거                                  



using AutoMoverPro;

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SandWormFSM_Attack1 : StateMachineBehaviour
{
     
     GameObject Player;
    AutoMover wormAutoMover;
    private float initialDistance; // 초기 플레이어와의 거리

    float Timer;
    private float timerr;
    public float roarInterval = 0.5f; // Under 사운드 재생 간격
    // OnStateEnter는 전환 시작시 호출되며 상태 기계가 이 상태를 평가하기 시작합니다.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Attack1Enter");
        animator.SetBool("Idle", false);
        Player = GameObject.FindGameObjectWithTag("Player");
        
        SandWormBoss.Instance.IsAttacking = true; //보스 공격상태 초기화
        initialDistance = Vector3.Distance(animator.gameObject.transform.position, Player.transform.position);
        //Debug.Log("초기거리!"+initialDistance);
        int distance = (int)(initialDistance*0.2f);
        Vector3 startpos = animator.transform.position+new Vector3(0,10,0);
        Vector3 wavestart = animator.transform.position + new Vector3(0, 10, 0);
        Vector3 waveend = Player.transform.position;
        Vector3 endpos = new Vector3(0, -20, 0) + Player.transform.position + ((Player.transform.position - animator.transform.position) * 1.2f);

        SandWormBoss.Instance.MakingSinMovement(animator.gameObject, 4, distance, startpos, wavestart,waveend ,endpos , 0, "", AutoMoverLoopingStyle.loop);

        wormAutoMover = animator.GetComponent<AutoMover>();
        Timer = 0;
    }

    // OnStateUpdate는 OnStateEnter와 OnStateExit 콜백 사이의 각 Update 프레임에서 호출됩니다.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // float distance = Vector3.Distance(worm.transform.position, PlayerOriginPos);
        float distance = Vector3.Distance(animator.gameObject.transform.position, Player.transform.position);

        timerr += Time.deltaTime;

        if (timerr >= roarInterval)
        {
            SoundManager.Instance.PlayEffect("Under");
            timerr = 0f; // 타이머를 리셋하여 재생 간격마다 사운드를 재생
        }


        Timer += Time.deltaTime;
        // 만약 초기 거리보다 커지면 Ingage 상태를 종료
        if (Timer > distance + 0.5f)
        {
           if(wormAutoMover != null)
            {
            Destroy(wormAutoMover);

            }
            
            SandWormBoss.Instance.IsAttacking = false;
            SandWormBoss.Instance.StartPattern("Idle");

            SandWormBoss.Instance.StopPattern("SWAttack1");
           

        }
    }

    // OnStateExit는 전환이 끝나고 상태 기계가 이 상태를 평가를 완료할 때 호출됩니다.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
  
    // OnStateMove는 Animator.OnAnimatorMove() 바로 다음에 호출됩니다.
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // 루트 모션을 처리하고 영향을 미치는 코드를 구현합니다.
    //}

    // OnStateIK는 Animator.OnAnimatorIK() 바로 다음에 호출됩니다.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // 애니메이션 IK (역위학)를 설정하는 코드를 구현합니다.
    //}
}