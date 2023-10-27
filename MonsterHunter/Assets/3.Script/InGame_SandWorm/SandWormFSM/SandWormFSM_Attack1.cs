//������ ������鼭 �����ϴ°�                                  



using AutoMoverPro;

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SandWormFSM_Attack1 : StateMachineBehaviour
{
     
     GameObject Player;
    AutoMover wormAutoMover;
    private float initialDistance; // �ʱ� �÷��̾���� �Ÿ�

    float Timer;
    private float timerr;
    public float roarInterval = 0.5f; // Under ���� ��� ����
    // OnStateEnter�� ��ȯ ���۽� ȣ��Ǹ� ���� ��谡 �� ���¸� ���ϱ� �����մϴ�.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Attack1Enter");
        animator.SetBool("Idle", false);
        Player = GameObject.FindGameObjectWithTag("Player");
        
        SandWormBoss.Instance.IsAttacking = true; //���� ���ݻ��� �ʱ�ȭ
        initialDistance = Vector3.Distance(animator.gameObject.transform.position, Player.transform.position);
        //Debug.Log("�ʱ�Ÿ�!"+initialDistance);
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

        timerr += Time.deltaTime;

        if (timerr >= roarInterval)
        {
            SoundManager.Instance.PlayEffect("Under");
            timerr = 0f; // Ÿ�̸Ӹ� �����Ͽ� ��� ���ݸ��� ���带 ���
        }


        Timer += Time.deltaTime;
        // ���� �ʱ� �Ÿ����� Ŀ���� Ingage ���¸� ����
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

    // OnStateExit�� ��ȯ�� ������ ���� ��谡 �� ���¸� �򰡸� �Ϸ��� �� ȣ��˴ϴ�.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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