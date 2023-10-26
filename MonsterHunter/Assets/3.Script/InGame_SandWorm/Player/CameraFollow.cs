using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target; // 카메라가 따라다닐 대상
    [SerializeField] Vector3 offset; // 카메라와 대상 사이의 오프셋
    [SerializeField] Vector2 clampAxis = new Vector2(60, 60); // 카메라 회전 각도 제한 (X: 최소, Y: 최대)

    [SerializeField] float follow_smoothing = 5; // 카메라가 대상을 따라가는 부드러움 정도
    [SerializeField] float rotate_Smoothing = 5; // 카메라 회전 부드러움 정도
    [SerializeField] float senstivity = 60; // 마우스 감도

    [SerializeField] float rotX , rotY; // 카메라 회전값 (X와 Y 각)
    Transform cam; // 메인 카메라의 Transform

    public bool lockedTarget; // 타겟 잠금 여부




    void Start()
    {
        Cursor.visible = false; // 커서를 숨김
        Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
        cam = Camera.main.transform; // 메인 카메라의 Transform 가져옴
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    void Update()
    {
        Vector3 target_P = target.position + offset; // 대상 위치와 오프셋을 더해서 카메라 타겟 위치 설정
        transform.position = Vector3.Lerp(transform.position, target_P, follow_smoothing * Time.deltaTime); // 부드러운 이동으로 카메라를 타겟 위치로 이동
     //transform.position = Vector3.Lerp(transform.position, target_P, follow_smoothing); // 부드러운 이동으로 카메라를 타겟 위치로 이동
       // transform.position = target_P; // 부드러운 이동으로 카메라를 타겟 위치로 이동

        if (!lockedTarget)
        {
            CameraTargetRotation(); // 일반적인 카메라 회전 동작
           
        }
        else
        {
            
            LookAtTarget(); // 타겟을 바라보는 카메라 회전 동작
        }

      
    }
    public float x;
    public float y;

    //bool toggle;
    //void FreeLook()
    //{
    //    rotY = 1.8f; // Y 회전값을 고정값으로 설정 (타겟을 고정으로 바라봄)
    //}

    void CameraTargetRotation()
    {
        // 나중에 초기 카메라값 넣기

        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // 마우스 움직임 얻기
        rotX += (mouseAxis.x * senstivity) * Time.deltaTime; // X 회전값 갱신
        rotY -= ((mouseAxis.y * senstivity) * Time.deltaTime); // Y 회전값 갱신
       
        
        rotY = 100f; //나중에 주석처리 해주기


        rotY = Mathf.Clamp(rotY, 0,180); // Y 회전값을 최소와 최대 값 사이로 제한 (-clampAxis.x를 사용하여 음수 값으로 제한)

        Quaternion localRotation = Quaternion.Euler(rotY, rotX, 0); // X와 Y 회전값을 이용한 회전 Quaternion 생성
        transform.rotation = Quaternion.Slerp(transform.rotation, localRotation, rotate_Smoothing); // 부드러운 회전 적용
        
    }

    [SerializeField] float PlusPoint =0;
    void LookAtTarget()
    {
        transform.rotation = cam.rotation; // 타겟을 바라보는 카메라 회전을 메인 카메라의 회전값으로 설정
        Vector3 r = cam.eulerAngles; // 메인 카메라의 각도 벡터
        rotX = r.y; // X 회전값을 메인 카메라의 Y 각도로 설정
        rotY = 1.8f + PlusPoint; // Y 회전값을 고정값으로 설정 (타겟을 고정으로 바라봄)
       
    }
}