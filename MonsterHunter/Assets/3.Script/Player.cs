using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Nowspeed;      // ĳ���� ���� ������ ���ǵ�.
    public float speed;      // ĳ���� ������ ���ǵ�.
    public float RunningSpeed; //ĳ���� �ٱ� ���ǵ�
    public float jumpSpeed; // ĳ���� ���� ��.
    public float gravity;    // ĳ���Ϳ��� �ۿ��ϴ� �߷�.

    private CharacterController controller; // ���� ĳ���Ͱ� �������ִ� ĳ���� ��Ʈ�ѷ� �ݶ��̴�.
    public Vector3 MoveDir;                // ĳ������ �����̴� ����.


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

        Cursor.visible = false; //Ŀ�� �����
        Cursor.lockState = CursorLockMode.Locked; //Ŀ�� ������Ű��

        cinemachineFreeLook= cam.GetComponent<CinemachineFreeLook>();

    }
    public float rotationSpeed = 5.0f;
    public Vector3 cameraForward;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Cursor.visible = !Cursor.visible; 
            Cursor.lockState = CursorLockMode.None; //Ŀ�� ������Ű��
            if(!Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked; //Ŀ�� ������Ű��
            }
        }

        //if (Input.GetKey(KeyCode.W))
        //{
        //    Camera mainCamera = Camera.main;

        //    if (mainCamera != null)
        //    {
        //        // ī�޶� �ٶ󺸴� ������ ����ϴ�.
        //        cameraForward = mainCamera.transform.forward;
        //        cameraForward.y = 0; // Y ���� 0���� �����Ͽ� ���� �������� ����ϴ�.

        //        // ���� �������� �÷��̾ ȸ����ŵ�ϴ�.
        //        Quaternion newRotation = Quaternion.LookRotation(cameraForward);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        //    }
        //}
       

        // ���� ĳ���Ͱ� ���� �ִ°�?
        if (controller.isGrounded)
        {
            // ��, �Ʒ� ������ ����. 
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            

            // ���͸� ���� ��ǥ�� ���ؿ��� ���� ��ǥ�� �������� ��ȯ�Ѵ�.
            MoveDir = transform.TransformDirection(MoveDir);

            // ���ǵ� ����.
            MoveDir *= Nowspeed;

            // ĳ���� ����
            if (Input.GetButton("Jump"))
                MoveDir.y = jumpSpeed;
            
        }
        
        // ĳ���Ϳ� �߷� ����.
        MoveDir.y -= gravity * Time.deltaTime;

        // ĳ���� ������.
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
