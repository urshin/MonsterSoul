//������ ������鼭 �����ϴ°�                                  



using AutoMoverPro;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormFSM_Attack1 : StateMachineBehaviour
{
     
     GameObject Player;
    AutoMover wormAutoMover;
    private float initialDistance; // �ʱ� �÷��̾���� �Ÿ�

    float Timer;

    // OnStateEnter�� ��ȯ ���۽� ȣ��Ǹ� ���� ��谡 �� ���¸� ���ϱ� �����մϴ�.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Attack1Enter");
        animator.SetBool("Idle", false);
        Player = GameObject.FindGameObjectWithTag("Player");
        
        SandWormBoss.Instance.IsAttacking = true; //���� ���ݻ��� �ʱ�ȭ
        initialDistance = Vector3.Distance(animator.gameObject.transform.position, Player.transform.position);
        Debug.Log("�ʱ�Ÿ�!"+initialDistance);
        int distance = (int)(initialDistance*0.2f);
        Vector3 startpos = animator.transform.position+new Vector3(0,10,0);
        Vector3 wavestart = animator.transform.position + new Vector3(0, 10, 0);
        Vector3 waveend = Player.transform.position;
        Vector3 endpos = new Vector3(0, -20, 0) + Player.transform.position + ((Player.transform.position - animator.transform.position) * 1.2f);

        SandWormBoss.Instance.MakingSinMovement(animator.gameObject, 4, distance, startpos, wavestart,waveend ,endpos , 0, "", AutoMoverLoopingStyle.loop);

        wormAutoMover = animator.GetComponent<AutoMover>();
        Timer = 0;
    }

    // OnStateUpdate�� OnStateEnter�� OnStateExit �ݹ� ������ �� Update �����ӿ��� ȣ��˴ϴ�.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // float distance = Vector3.Distance(worm.transform.position, PlayerOriginPos);
        float distance = Vector3.Distance(animator.gameObject.transform.position, Player.transform.position);
        Timer += Time.deltaTime;    
        // ���� �ʱ� �Ÿ����� Ŀ���� Ingage ���¸� ����
        if (Timer > 4.5f)
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

    // OnStateExit�� ��ȯ�� ������ ���� ��谡 �� ���¸� �򰡸� �Ϸ��� �� ȣ��˴ϴ�.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
    //private void MakingSinMovement(float _sideVectorMagnitude, int _HowMany, float _Speed)
    //{
    //    wormAutoMover.RunOnStart = false;

    //    wormAutoMover.AnchorPointSpace = AutoMoverAnchorPointSpace.world;

    //    wormAutoMover.FaceForward = true;
    //    wormAutoMover.DynamicUpVector = false;
    //    wormAutoMover.AddAnchorPoint(worm.transform.position + new Vector3(0, 0, 0), new Vector3(0, 0, 0), worm.transform.localScale);
    //    wormAutoMover.AddAnchorPoint(SandWormBoss.Instance.SetPosition.position+new Vector3(0,20,0) , new Vector3(0, 0, 0), worm.transform.localScale);
    //    // Sin �Լ��� ����Ͽ� X ��ǥ�� �����Ͽ� �翷���� ������


    //    for (int i = 1; i <= _HowMany; i++)
    //    {

    //        Vector3 position = (i * (((Player.transform.position - worm.transform.position) / (_HowMany + 1f))) + worm.transform.position);


    //        // �� ������Ʈ�� �÷��̾� ������Ʈ�� �ٶ󺸴� ������ ����մϴ�.
    //        Vector3 directionToPlayer = Player.transform.position - worm.transform.position;

    //        // ���� ���͸� ����մϴ�. �̸� ���� x�� z ���� ��Ҹ� ��ȯ�ϰ� �� �� �ϳ��� ��ȣ�� �ٲپ� ����մϴ�.
    //        Vector3 sideVector = new Vector3(-directionToPlayer.z, 0, directionToPlayer.x).normalized;

           
    //        // �ʿ��� ��� sideVector�� ũ�⸦ ������ �� �ֽ��ϴ�.
    //        float sideVectorMagnitude = _sideVectorMagnitude; // �ʿ��� ũ�⿡ �°� �� ���� �����ϼ���.
    //        sideVector *= sideVectorMagnitude;

    //        if (i % 2 == 0)
    //        {
    //            // ���� sideVector�� ��ġ�� �߰��� �� �ֽ��ϴ�.
    //            position += sideVector;

    //        }
    //        else
    //        {
    //            position -= sideVector;
    //        }

    //        // ������ �ڵ�...


    //        Vector3 rotation = Player.transform.eulerAngles;
    //        Vector3 scale = worm.transform.localScale;
    //        wormAutoMover.AddAnchorPoint(position, rotation, scale);
    //    }
    //    wormAutoMover.Length = _Speed;
    //    wormAutoMover.LoopingStyle = AutoMoverLoopingStyle.repeat;
    //    wormAutoMover.CurveStyle = AutoMoverCurve.SplineThroughPoints;


    //    wormAutoMover.AddAnchorPoint(new Vector3(0,-10,0)+ Player.transform.position +((Player.transform.position- worm.transform.position)*1.2f), Player.transform.eulerAngles, Player.transform.localScale);

    //    wormAutoMover.StartMoving();
        
    //}
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