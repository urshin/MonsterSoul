using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Nowspeed;      // 캐릭터 현재 움직임 스피드.
    public float speed;      // 캐릭터 움직임 스피드.
    public float RunningSpeed; //캐릭터 뛰기 스피드
    public float jumpSpeed; // 캐릭터 점프 힘.
    public float gravity;    // 캐릭터에게 작용하는 중력.

    private CharacterController controller; // 현재 캐릭터가 가지고있는 캐릭터 컨트롤러 콜라이더.
    public Vector3 MoveDir;                // 캐릭터의 움직이는 방향.


    public GameObject cam;
    private CinemachineFreeLook cinemachineFreeLook;



    void Start()
    {
        speed = 4.0f;
        RunningSpeed = speed * 1.5f;
        Nowspeed = speed;
        jumpSpeed = 8.0f;
        gravity = 20.0f;

        MoveDir = Vector3.zero;
        controller = GetComponent<CharacterController>();

        Cursor.visible = false; //커서 숨기기
        Cursor.lockState = CursorLockMode.Locked; //커서 고정시키기

        cinemachineFreeLook= cam.GetComponent<CinemachineFreeLook>();

    }
    public float rotationSpeed = 5.0f;
    public Vector3 cameraForward;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Cursor.visible = !Cursor.visible; 
            Cursor.lockState = CursorLockMode.None; //커서 고정시키기
            if(!Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked; //커서 고정시키기
            }
        }

        //if (Input.GetKey(KeyCode.W))
        //{
        //    Camera mainCamera = Camera.main;

        //    if (mainCamera != null)
        //    {
        //        // 카메라가 바라보는 방향을 얻습니다.
        //        cameraForward = mainCamera.transform.forward;
        //        cameraForward.y = 0; // Y 값만 0으로 설정하여 수평 방향으로 만듭니다.

        //        // 수평 방향으로 플레이어를 회전시킵니다.
        //        Quaternion newRotation = Quaternion.LookRotation(cameraForward);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        //    }
        //}
       

        // 현재 캐릭터가 땅에 있는가?
        if (controller.isGrounded)
        {
            // 위, 아래 움직임 셋팅. 
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            

            // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
            MoveDir = transform.TransformDirection(MoveDir);

            // 스피드 증가.
            MoveDir *= Nowspeed;

            // 캐릭터 점프
            if (Input.GetButton("Jump"))
                MoveDir.y = jumpSpeed;
            
        }
        
        // 캐릭터에 중력 적용.
        MoveDir.y -= gravity * Time.deltaTime;

        // 캐릭터 움직임.
        controller.Move(MoveDir * Time.deltaTime);
        


        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    Nowspeed = RunningSpeed;
        //}
        //else
        //{
        //    Nowspeed = speed;
        //}
    }
}
