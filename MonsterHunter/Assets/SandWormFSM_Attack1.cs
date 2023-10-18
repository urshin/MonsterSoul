//옆으로 헤엄지면서 공격하는거



using AutoMoverPro;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormFSM_Attack1 : StateMachineBehaviour
{
     SandWormBoss sandWorm; //샌드웜 참조
     GameObject Player;
    AutoMover wormAutoMover;
     GameObject worm;
    private float initialDistance; // 초기 플레이어와의 거리
    private Vector3 PlayerOriginPos;
    // OnStateEnter는 전환 시작시 호출되며 상태 기계가 이 상태를 평가하기 시작합니다.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        animator.SetBool("Idle", false);
        Player = GameObject.FindGameObjectWithTag("Player");
        worm = animator.gameObject;

        wormAutoMover = animator.gameObject.AddComponent<AutoMover>();
        initialDistance = Vector3.Distance(worm.transform.position, Player.transform.position);
        Debug.Log("초기거리!"+initialDistance);
        MakingSinMovement(3, 3, 2);
        PlayerOriginPos = Player.transform.position;
    }
    
    // OnStateUpdate는 OnStateEnter와 OnStateExit 콜백 사이의 각 Update 프레임에서 호출됩니다.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // float distance = Vector3.Distance(worm.transform.position, PlayerOriginPos);
        float distance = Vector3.Distance(animator.transform.position, Player.transform.position);
        Debug.Log(distance);
        // 만약 초기 거리보다 커지면 Ingage 상태를 종료
        if (distance <=1)
        {
            
            animator.SetBool("SWAttack1", false);
            animator.SetBool("Idle", true);

        }
    }

    // OnStateExit는 전환이 끝나고 상태 기계가 이 상태를 평가를 완료할 때 호출됩니다.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
    private void MakingSinMovement(float _sideVectorMagnitude, int _HowMany, float _Speed)
    {
        wormAutoMover.RunOnStart = false;

        wormAutoMover.AnchorPointSpace = AutoMoverAnchorPointSpace.world;

        wormAutoMover.FaceForward = true;
        wormAutoMover.DynamicUpVector = false;
        wormAutoMover.AddAnchorPoint(worm.transform.position + new Vector3(0, 0, 0), new Vector3(0, 0, 0), worm.transform.localScale);
        // Sin 함수를 사용하여 X 좌표를 조정하여 양옆으로 움직임


        for (int i = 1; i <= _HowMany; i++)
        {

            Vector3 position = (i * (((Player.transform.position - worm.transform.position) / (_HowMany + 1f))) + worm.transform.position);


            // 이 오브젝트가 플레이어 오브젝트를 바라보는 방향을 계산합니다.
            Vector3 directionToPlayer = Player.transform.position - worm.transform.position;

            // 수직 벡터를 계산합니다. 이를 위해 x와 z 구성 요소를 교환하고 그 중 하나를 부호를 바꾸어 사용합니다.
            Vector3 sideVector = new Vector3(-directionToPlayer.z, 0, directionToPlayer.x).normalized;

            // 필요한 경우 sideVector의 크기를 조절할 수 있습니다.
            float sideVectorMagnitude = _sideVectorMagnitude; // 필요한 크기에 맞게 이 값을 조절하세요.
            sideVector *= sideVectorMagnitude;

            if (i % 2 == 0)
            {
                // 이제 sideVector를 위치에 추가할 수 있습니다.
                position += sideVector;

            }
            else
            {
                position -= sideVector;
            }

            // 나머지 코드...


            Vector3 rotation = Player.transform.eulerAngles;
            Vector3 scale = worm.transform.localScale;
            wormAutoMover.AddAnchorPoint(position, rotation, scale);
        }
        wormAutoMover.Length = _Speed;
        wormAutoMover.LoopingStyle = AutoMoverLoopingStyle.repeat;
        wormAutoMover.CurveStyle = AutoMoverCurve.SplineThroughPoints;


        wormAutoMover.AddAnchorPoint(Player.transform.position, Player.transform.eulerAngles, Player.transform.localScale);



        wormAutoMover.Length = 10f;
        wormAutoMover.LoopingStyle = AutoMoverLoopingStyle.repeat;
        wormAutoMover.CurveStyle = AutoMoverCurve.SplineThroughPoints;

        wormAutoMover.StartMoving();
        
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