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

    public float movementSpeed = 2.0f; // �̵� �ӵ�

    // OnStateEnter�� ��ȯ ���� �� ȣ��Ǹ� ���� ��谡 �� ���¸� ���ϱ� ������ ���Դϴ�.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("Idle"))
        {
            animator.transform.position = UnderSetPosition.position;
            // ���� ��ġ�� SetPosition ������ �Ÿ��� ����մϴ�.
            journeyLength = Vector3.Distance(animator.transform.position, SetPosition.position);
            startTime = Time.time;
            isMoving = true;
        }
    }

    // OnStateUpdate�� OnStateEnter�� OnStateExit �ݹ� ������ �� ������Ʈ �����ӿ��� ȣ��˴ϴ�.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isMoving)
        {
            // ���� �ð��� ���� �̵��� �Ÿ��� ����մϴ�.
            float distanceCovered = (Time.time - startTime) * movementSpeed;

            // �Ϸ�� ������ �м��� ����մϴ�.
            float fractionOfJourney = distanceCovered / journeyLength;

            // �ִϸ������� ��ġ�� SetPosition���� ���� �̵��մϴ�.
            animator.transform.position = Vector3.Lerp(animator.transform.position, SetPosition.position, fractionOfJourney);

            // ������ �Ϸ�Ǿ����� Ȯ���մϴ�.
            if (fractionOfJourney >= 1.0f)
            {
                isMoving = false;
                animator.SetBool("Idle", false);
                //���߿� �ð� üũ�ϰ� ���� ��� �����̱�



            }
        }
    }

    // OnStateExit�� ��ȯ ���� �� ȣ��Ǹ� ���� ��谡 �� ���¸� �򰡸� �Ϸ��� ���Դϴ�.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Idle", false);
    }

    // OnStateMove�� Animator.OnAnimatorMove() ���� �ٷ� ȣ��˴ϴ�.
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // ��Ʈ ����� ó���ϰ� ������ ��ġ�� �ڵ带 �����մϴ�.
    //}

    // OnStateIK�� Animator.OnAnimatorIK() ���� �ٷ� ȣ��˴ϴ�.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // �ִϸ��̼� IK (�������)�� �����ϴ� �ڵ带 �����մϴ�.
    //}
}