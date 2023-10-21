//불타는 돌덩이 출력
using AutoMoverPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SandWormFSM_Attack3 : StateMachineBehaviour
{

    [SerializeField] GameObject[] Rock;
    AutoMover wormAutoMover;

    float radius;
    private float timerDuration = 2.5f; // 타이머의 기간(초)
    private float timeRemaining; // 남은 시간(초)
    private float lastTimeChecked; // 마지막으로 시간을 체크한 시간
    private float outputInterval = 0.3f; // 출력 간격(초)

    bool OktoShootingFire;
    float timer = 0;
   
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wormAutoMover = animator.GetComponent<AutoMover>();
        radius = 24;
        SandWormBoss.Instance.MakingSinMovement(animator. gameObject, 1, 4, animator.transform.position + new Vector3(radius * 2, 0, 0), animator.transform.position + new Vector3(0, radius, 0), animator.transform.position + new Vector3(-radius * 2, 0, 0), animator.transform.position + new Vector3(0, -100, 0), 3.8f, "circle", AutoMoverLoopingStyle.repeat);
        OktoShootingFire = true;

        SandWormBoss.Instance.IsAttacking = true;
        timeRemaining = timerDuration;
        lastTimeChecked = Time.time;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float elapsedTime = Time.time - lastTimeChecked;

        if (elapsedTime >= outputInterval && OktoShootingFire)
        {
            ShootingFire();
            lastTimeChecked = Time.time;
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0.0f)
            {
                if (wormAutoMover != null)
                {

                    Destroy(wormAutoMover);
                }
                SandWormBoss.Instance.StartPattern("Idle");
                SandWormBoss.Instance.IsAttacking = false;
                SandWormBoss.Instance.StopPattern("SWAttack3");
                OktoShootingFire = false;
               
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
    void ShootingFire()
    {
        for (int i = 0; i < Rock.Length; i++)
        {
            GameObject rock = Instantiate(Rock[i], SandWormBoss.Instance.anime_Nav.transform.position, Quaternion.identity);
            Destroy(rock, 4f);
        }
    }
    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
