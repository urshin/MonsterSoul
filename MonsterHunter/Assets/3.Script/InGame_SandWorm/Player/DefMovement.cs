using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class DefMovement : MonoBehaviour
{
    public static DefMovement Instance; // 이 클래스의 인스턴스를 다른 곳에서 참조할 수 있도록 정적 변수로 선언
    GameManager GM; // 게임 매니저 클래스의 인스턴스를 저장하는 변수

    // Awake 메소드는 게임 오브젝트가 활성화될 때 호출됩니다.
    public void Awake()
    {
        if (Instance == null) // 정적으로 자신을 체크함, null인 경우에만 실행
        {
            Instance = this; // 이후 자기 자신을 저장함.
        }
    }

    public CharacterController controller; // 캐릭터 컨트롤러 컴포넌트
    public Animator anim; // 애니메이터 컴포넌트
    public Transform cam; // 메인 카메라의 Transform

    public float speedSmoothVelocity; // 속도 부드럽게 변화시키는 데 사용되는 변수
    public float speedSmoothTime; // 속도 부드럽게 변화시키는 데 사용되는 시간
    public float currentSpeed; // 현재 이동 속도
    public float velocityY; // 수직 속도 (중력 적용)
    public Vector3 moveInput; // 이동 입력 벡터
    public Vector3 dir; // 이동 방향 벡터
    public float Nowspeed; // 캐릭터 현재 움직임 스피드.

    [Header("Settings")] // 나중에 플레이어에 따라 바뀜 // start에서 참조해오기
    public float gravity = 25f; // 중력
    public float moveSpeed = 4f; // 이동 속도
    public float RunningSpeed = 7f; // 캐릭터 뛰기 스피드
    public float rotateSpeed = 3f; // 회전 속도
    public float JumpSpeed = 5f; // 점프

    public bool lockMovement; // 이동 잠금 여부

    void Start()
    {
        GM = GameManager.Instance; // 게임 매니저의 인스턴스 가져오기
        Invoke("LateStart", 0.1f); // LateStart 메소드를 0.1초 뒤에 실행
        controller = GetComponent<CharacterController>(); // 캐릭터 컨트롤러 컴포넌트 가져오기
        cam = Camera.main.transform; // 메인 카메라의 Transform 가져오기
    }

    void LateStart()
    {
        anim = Player.Instance.PlayerAvatar.GetComponent<Animator>(); // 플레이어의 아바타에 연결된 애니메이터 컴포넌트 가져오기
        InitializedPlayerInfo(); // 플레이어 정보 초기화 메소드 호출
    }

    void InitializedPlayerInfo()
    {
        moveSpeed = GM.PlayerSpeed; // 게임 매니저에서 설정한 플레이어 이동 속도 가져오기
        Nowspeed = moveSpeed; // 이동 속도 초기화
        JumpSpeed = GM.PlayerJumpPower; // 게임 매니저에서 설정한 플레이어 점프 파워 가져오기
        RunningSpeed = moveSpeed * 1.4f; // 이동 속도에 기반한 뛰기 속도 설정
    }

    public void Update()
    {
        if (!GM.IsLoading) // 게임이 로딩 중이 아닌 경우에만 업데이트 메소드 실행
        {
            GetInput(); // 입력 받기

            if (Player.Instance.isPlayerBulling) // 공격받는 상태
            {
                if (Player.Instance.IsDown)
                {
                    anim.SetTrigger("Down"); // 애니메이터 트리거 "Down" 설정
                    Player.Instance.IsDown = false; // 플레이어 다운 상태 해제
                }
            }
            else
            {
                PlayerMovement(); // 플레이어 이동
            }

            if (!lockMovement) PlayerRotation(); // 이동이 잠긴 상태가 아니면 플레이어 회전
        }
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

        // 공격, 구르기 또는 점프 중이 아닌 경우에만 이동 입력을 처리
        if (anim.GetBool("IsAttack") || anim.GetBool("IsRoll") || anim.GetBool("IsJump") || anim.GetBool("IsBuff"))
        {
            // 이동 입력을 무시
        }
        else
        {
            Nowspeed = moveSpeed; // 현재 이동 속도 설정
            dir = (forward * moveInput.y + right * moveInput.x).normalized; // 입력을 기반으로 이동 방향 벡터 생성
        }
    }

    private void PlayerMovement()
    {
        currentSpeed = Mathf.SmoothDamp(currentSpeed, Nowspeed, ref speedSmoothVelocity, speedSmoothTime * Time.deltaTime); // 부드러운 속도 변화 계산
        if (velocityY > -10) velocityY -= Time.deltaTime * gravity; // 중력 적용

        Vector3 velocity = (dir * currentSpeed) + Vector3.up * velocityY; // 이동 속도 벡터 계산

        controller.Move(velocity * Time.deltaTime); // 이동 속도로 캐릭터 이동

        anim.SetBool("IsMoving", anim.GetFloat("Movement") >= 0.1f); // 움직이는 중인지 체크

        if (controller.isGrounded)
        {
            anim.SetBool("IsJump", false); // 점프 애니메이션 비활성화
            if (!anim.GetBool("IsAttack"))
            {
                if (Input.GetButton("Jump") && !anim.GetBool("IsRoll"))
                {
                    anim.SetBool("IsJump", true); // 점프 애니메이션 활성화
                }
                if (!anim.GetBool("IsJump") && !anim.GetBool("IsRoll"))
                {
                    if (Input.GetKeyDown(KeyCode.R) && Player.Instance.InventoryItems.Count > 0)
                    {
                        Player.Instance.UsingItem(); // 아이템 사용
                        anim.SetBool("IsBuff", true); // 버프 애니메이션 활성화
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!anim.GetBool("IsRoll"))
                {
                    Roll(); // 구르기 메소드 호출
                    anim.SetTrigger("Rolling"); // 구르기 애니메이션 트리거 설정
                }
            }
        }

        HandleMouseInput(KeyCode.Mouse0, "LeftClick"); // 마우스 왼쪽 버튼 입력 처리
        HandleMouseInput(KeyCode.Mouse1, "RightClick"); // 마우스 오른쪽 버튼 입력 처리

        // 애니메이터의 파라미터 설정
        anim.SetFloat("Movement", dir.magnitude, 0.1f, Time.deltaTime);
        anim.SetFloat("Horizontal", moveInput.x, 0.1f, Time.deltaTime);
        anim.SetFloat("Vertical", moveInput.y, 0.1f, Time.deltaTime);
    }

    void HandleMouseInput(KeyCode mouseButton, string boolName) // 콤보
    {
        if (Input.GetKeyDown(mouseButton))
        {
            anim.SetBool(boolName, true); // 마우스 버튼 입력에 따른 불리언 변수 설정
            if (anim.GetBool("AbleCombo"))
            {
                anim.SetTrigger("GoNextAttack"); // 다음 공격 트리거 설정
                anim.SetBool("AbleCombo", false); // 콤보 가능 여부 비활성화
            }
        }
        if (Input.GetKeyUp(mouseButton))
        {
            anim.SetBool(boolName, false); // 마우스 버튼 뗄 때 불리언 변수 비활성화
        }
    }

    public void Roll()
    {
        StartCoroutine(Roll_Movement()); // 구르기 메소드 시작
    }

    private IEnumerator Roll_Movement()
    {
        anim.SetBool("IsRoll", true); // 구르기 애니메이션 활성화

        // 구르는 동안의 방향 벡터 저장
        Vector3 rollDir = (cam.forward * moveInput.y + cam.right * moveInput.x).normalized;
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
    }

    private void PlayerRotation()
    {
        if (dir.magnitude == 0) return; // 이동 방향이 없으면 회전하지 않음

        Vector3 rotDir = new Vector3(dir.x, dir.y, dir.z); // 회전 방향 벡터 생성
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), Time.deltaTime * rotateSpeed); // 부드러운 회전 적용
    }
}