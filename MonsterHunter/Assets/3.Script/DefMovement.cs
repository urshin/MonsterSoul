using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefMovement : MonoBehaviour
{
    CharacterController controller; // 캐릭터 컨트롤러 컴포넌트
    Animator anim; // 애니메이터 컴포넌트
    Transform cam; // 메인 카메라의 Transform

    float speedSmoothVelocity; // 속도 부드럽게 변화시키는 데 사용되는 변수
    float speedSmoothTime; // 속도 부드럽게 변화시키는 데 사용되는 시간
    [SerializeField] float currentSpeed; // 현재 이동 속도
    [SerializeField] float velocityY; // 수직 속도 (중력 적용)
    Vector3 moveInput; // 이동 입력 벡터
    Vector3 dir; // 이동 방향 벡터

    [Header("Settings")]
    [SerializeField] float gravity = 25f; // 중력
    [SerializeField] float Nowspeed;      // 캐릭터 현재 움직임 스피드.
    [SerializeField] float moveSpeed = 4f; // 이동 속도
    [SerializeField] float RunningSpeed = 7f; //캐릭터 뛰기 스피드
    [SerializeField] float rotateSpeed = 3f; // 회전 속도
    [SerializeField] float JumpSpeed = 5f; //점프 

    public bool lockMovement; // 이동 잠금 여부

    void Start()
    {
        //anim = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
        anim = GetComponentInChildren<Animator>(); // 애니메이터 컴포넌트 가져오기
        controller = GetComponent<CharacterController>(); // 캐릭터 컨트롤러 컴포넌트 가져오기
        cam = Camera.main.transform; // 메인 카메라의 Transform 가져오기
        Nowspeed = moveSpeed; //이동속도 초기화
    }

    void Update()
    {
        GetInput(); // 입력 받기
        PlayerMovement(); // 플레이어 이동
        if (!lockMovement) PlayerRotation(); // 이동이 잠긴 상태가 아니면 플레이어 회전
    }

    private void GetInput()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // 수평과 수직 입력 받기

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize(); // 정규화하여 방향 벡터 생성
        right.Normalize(); // 정규화하여 방향 벡터 생성

        dir = (forward * moveInput.y + right * moveInput.x).normalized; // 입력을 기반으로 이동 방향 벡터 생성
    }

    private void PlayerMovement()
    {
        GameManager.Instance.PlayerCurrentState = PlayerState.Move; //플레이어 현재상태
        currentSpeed = Mathf.SmoothDamp(currentSpeed, Nowspeed, ref speedSmoothVelocity, speedSmoothTime * Time.deltaTime); // 부드러운 속도 변화 계산

        if (velocityY > -10) velocityY -= Time.deltaTime * gravity; // 중력 적용

        Vector3 velocity = (dir * currentSpeed) + Vector3.up * velocityY; // 이동 속도 벡터 계산

        controller.Move(velocity * Time.deltaTime); // 이동 속도로 캐릭터 이동

        if (controller.isGrounded)
        {
            anim.SetBool("Jump", false);

            if (Input.GetButton("Jump"))
            {

                velocityY = JumpSpeed; // 수직 속도를 점프 속도로 설정
                anim.SetBool("Jump", true);
                //anim.Play("Jump");

            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!anim.GetBool("Roll"))
                {
                    
                    StartCoroutine(Roll_Movement());
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //NormalAttack();
            anim.SetBool("LeftClick",true);
            if (anim.GetBool("AbleCombo"))
            {
                anim.SetTrigger("GoNextAttack");
                anim.SetBool("AbleCombo", false);
            }
        }
        if (Input.GetKeyUp (KeyCode.Mouse0))
            {
            anim.SetBool("LeftClick", false);

        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //NormalAttack();
            anim.SetBool("RightClick", true);
            if (anim.GetBool("AbleCombo"))
            {
                anim.SetTrigger("GoNextAttack");
                anim.SetBool("AbleCombo", false);
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            anim.SetBool("RightClick", false);

        }



        // 애니메이터의 파라미터 설정 
        anim.SetFloat("Movement", dir.magnitude, 0.1f, Time.deltaTime);
        anim.SetFloat("Horizontal", moveInput.x, 0.1f, Time.deltaTime);
        anim.SetFloat("Vertical", moveInput.y, 0.1f, Time.deltaTime);
    }

   


    IEnumerator Roll_Movement()
    {
        anim.SetBool("Roll", true);

        // 구르는 동안의 방향 벡터 저장
        Vector3 rollDir = dir;
        rollDir.y = 0; // Y값을 0으로 설정하여 수직 이동 방향을 무시

        // 목표 이동 거리 설정
        float rollDistance = 5.0f;

        // 시작 지점 저장
        Vector3 startPos = transform.position;

        // 목표 지점 계산
        Vector3 endPos = startPos + rollDir.normalized * rollDistance;

        while (Vector3.Distance(transform.position, endPos) > 1f)
        {
            // 계산된 목표 지점 방향으로 이동
            Vector3 moveDirection = (endPos - transform.position).normalized;
            controller.Move(moveDirection * (moveSpeed + (moveSpeed / 2)) * Time.deltaTime);
            yield return null;
        }

        if (Vector3.Distance(transform.position, endPos) <= 1f)
        {
            anim.SetBool("Roll", false);
        }
    }

    private void PlayerRotation()
    {
        if (dir.magnitude == 0) return; // 이동 방향이 없으면 회전하지 않음

        Vector3 rotDir = new Vector3(dir.x, dir.y, dir.z); // 회전 방향 벡터 생성
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), Time.deltaTime * rotateSpeed); // 부드러운 회전 적용
    }
}