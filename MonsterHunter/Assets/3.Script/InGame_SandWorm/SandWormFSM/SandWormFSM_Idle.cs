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



    // OnStateEnter�� ��ȯ ���� �� ȣ��Ǹ� ���� ��谡 �� ���¸� ���ϱ� ������ ���Դϴ�.
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

    // OnStateUpdate�� OnStateEnter�� OnStateExit �ݹ� ������ �� ������Ʈ �����ӿ��� ȣ��˴ϴ�.
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
                // 30�̸��϶�

            }

            //���߿� ���� �Լ� ������
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

    // OnStateExit�� ��ȯ ���� �� ȣ��Ǹ� ���� ��谡 �� ���¸� �򰡸� �Ϸ��� ���Դϴ�.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("Idle", false);
    }


}