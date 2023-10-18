//������ ������鼭 �����ϴ°�



using AutoMoverPro;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormFSM_Attack1 : StateMachineBehaviour
{
     SandWormBoss sandWorm; //����� ����
     GameObject Player;
    AutoMover wormAutoMover;
     GameObject worm;
    private float initialDistance; // �ʱ� �÷��̾���� �Ÿ�
    private Vector3 PlayerOriginPos;
    // OnStateEnter�� ��ȯ ���۽� ȣ��Ǹ� ���� ��谡 �� ���¸� ���ϱ� �����մϴ�.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        animator.SetBool("Idle", false);
        Player = GameObject.FindGameObjectWithTag("Player");
        worm = animator.gameObject;

        wormAutoMover = animator.gameObject.AddComponent<AutoMover>();
        initialDistance = Vector3.Distance(worm.transform.position, Player.transform.position);
        Debug.Log("�ʱ�Ÿ�!"+initialDistance);
        MakingSinMovement(3, 3, 2);
        PlayerOriginPos = Player.transform.position;
    }
    
    // OnStateUpdate�� OnStateEnter�� OnStateExit �ݹ� ������ �� Update �����ӿ��� ȣ��˴ϴ�.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // float distance = Vector3.Distance(worm.transform.position, PlayerOriginPos);
        float distance = Vector3.Distance(animator.transform.position, Player.transform.position);
        Debug.Log(distance);
        // ���� �ʱ� �Ÿ����� Ŀ���� Ingage ���¸� ����
        if (distance <=1)
        {
            
            animator.SetBool("SWAttack1", false);
            animator.SetBool("Idle", true);

        }
    }

    // OnStateExit�� ��ȯ�� ������ ���� ��谡 �� ���¸� �򰡸� �Ϸ��� �� ȣ��˴ϴ�.
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
        // Sin �Լ��� ����Ͽ� X ��ǥ�� �����Ͽ� �翷���� ������


        for (int i = 1; i <= _HowMany; i++)
        {

            Vector3 position = (i * (((Player.transform.position - worm.transform.position) / (_HowMany + 1f))) + worm.transform.position);


            // �� ������Ʈ�� �÷��̾� ������Ʈ�� �ٶ󺸴� ������ ����մϴ�.
            Vector3 directionToPlayer = Player.transform.position - worm.transform.position;

            // ���� ���͸� ����մϴ�. �̸� ���� x�� z ���� ��Ҹ� ��ȯ�ϰ� �� �� �ϳ��� ��ȣ�� �ٲپ� ����մϴ�.
            Vector3 sideVector = new Vector3(-directionToPlayer.z, 0, directionToPlayer.x).normalized;

            // �ʿ��� ��� sideVector�� ũ�⸦ ������ �� �ֽ��ϴ�.
            float sideVectorMagnitude = _sideVectorMagnitude; // �ʿ��� ũ�⿡ �°� �� ���� �����ϼ���.
            sideVector *= sideVectorMagnitude;

            if (i % 2 == 0)
            {
                // ���� sideVector�� ��ġ�� �߰��� �� �ֽ��ϴ�.
                position += sideVector;

            }
            else
            {
                position -= sideVector;
            }

            // ������ �ڵ�...


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
    // OnStateMove�� Animator.OnAnimatorMove() �ٷ� ������ ȣ��˴ϴ�.
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // ��Ʈ ����� ó���ϰ� ������ ��ġ�� �ڵ带 �����մϴ�.
    //}

    // OnStateIK�� Animator.OnAnimatorIK() �ٷ� ������ ȣ��˴ϴ�.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // �ִϸ��̼� IK (������)�� �����ϴ� �ڵ带 �����մϴ�.
    //}
}