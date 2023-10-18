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

    public float UndermovementSpeed = 200.0f; // �̵� �ӵ�
    public float movementSpeed = 2.0f; // �̵� �ӵ�

    [SerializeField] float timer;
    [SerializeField] float WaitingTime;

    // OnStateEnter�� ��ȯ ���� �� ȣ��Ǹ� ���� ��谡 �� ���¸� ���ϱ� ������ ���Դϴ�.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetPosition = GameObject.FindGameObjectWithTag("EnemyUpPos").transform;
        UnderSetPosition = GameObject.FindGameObjectWithTag("EnemyUnderPos").transform;

        animator.SetBool("AttackTime", false);
        if (animator.GetBool("Idle"))
        {

            // ���� ��ġ�� SetPosition ������ �Ÿ��� ����մϴ�.
            UnderSetPositionjourneyLength = Vector3.Distance(animator.transform.position, UnderSetPosition.position);
            startTime = Time.time;
            isMoving_UnderSetPosition = true;

            SetPositionjourneyLength = Vector3.Distance(UnderSetPosition.position, SetPosition.position);
            startTime = Time.time;
            isMoving_SetPosition = false;


            timer = 0;

         

        }
    }

    // OnStateUpdate�� OnStateEnter�� OnStateExit �ݹ� ������ �� ������Ʈ �����ӿ��� ȣ��˴ϴ�.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isMoving_UnderSetPosition)
        {

            // ���� �ð��� ���� �̵��� �Ÿ��� ����մϴ�.
            float distanceCovered = (Time.time - startTime) * UndermovementSpeed;

            // �Ϸ�� ������ �м��� ����մϴ�.
            float fractionOfJourney = distanceCovered / UnderSetPositionjourneyLength;

            // �ִϸ������� ��ġ�� SetPosition���� ���� �̵��մϴ�.
            animator.transform.position = Vector3.Lerp(animator.transform.position, UnderSetPosition.position, fractionOfJourney);

            // ������ �Ϸ�Ǿ����� Ȯ���մϴ�.
            if (fractionOfJourney >= 2.0f)
            {
                isMoving_UnderSetPosition = false;
                isMoving_SetPosition = true;
         
            }
        }

        if (isMoving_SetPosition)
        {

            // ���� �ð��� ���� �̵��� �Ÿ��� ����մϴ�.
            float distanceCovered = (Time.time - startTime) * movementSpeed;

            // �Ϸ�� ������ �м��� ����մϴ�.
            float fractionOfJourney = distanceCovered / SetPositionjourneyLength;

            // �ִϸ������� ��ġ�� SetPosition���� ���� �̵��մϴ�.
            animator.transform.position = Vector3.Lerp(animator.transform.position, SetPosition.position, fractionOfJourney);

            // ������ �Ϸ�Ǿ����� Ȯ���մϴ�.
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
                    // 30�̸��϶�

                }

                //���߿� ���� �Լ� ������
                animator.SetBool("SWAttack1",true);
                animator.SetBool("Idle", false);
                timer= 0;
            }
        
        }


    }

    // OnStateExit�� ��ȯ ���� �� ȣ��Ǹ� ���� ��谡 �� ���¸� �򰡸� �Ϸ��� ���Դϴ�.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("Idle", false);
    }

   
}