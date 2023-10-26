using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target; // ī�޶� ����ٴ� ���
    [SerializeField] Vector3 offset; // ī�޶�� ��� ������ ������
    [SerializeField] Vector2 clampAxis = new Vector2(60, 60); // ī�޶� ȸ�� ���� ���� (X: �ּ�, Y: �ִ�)

    [SerializeField] float follow_smoothing = 5; // ī�޶� ����� ���󰡴� �ε巯�� ����
    [SerializeField] float rotate_Smoothing = 5; // ī�޶� ȸ�� �ε巯�� ����
    [SerializeField] float senstivity = 60; // ���콺 ����

    [SerializeField] float rotX , rotY; // ī�޶� ȸ���� (X�� Y ��)
    Transform cam; // ���� ī�޶��� Transform

    public bool lockedTarget; // Ÿ�� ��� ����




    void Start()
    {
        Cursor.visible = false; // Ŀ���� ����
        Cursor.lockState = CursorLockMode.Locked; // Ŀ�� ���
        cam = Camera.main.transform; // ���� ī�޶��� Transform ������
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    void Update()
    {
        Vector3 target_P = target.position + offset; // ��� ��ġ�� �������� ���ؼ� ī�޶� Ÿ�� ��ġ ����
        transform.position = Vector3.Lerp(transform.position, target_P, follow_smoothing * Time.deltaTime); // �ε巯�� �̵����� ī�޶� Ÿ�� ��ġ�� �̵�
     //transform.position = Vector3.Lerp(transform.position, target_P, follow_smoothing); // �ε巯�� �̵����� ī�޶� Ÿ�� ��ġ�� �̵�
       // transform.position = target_P; // �ε巯�� �̵����� ī�޶� Ÿ�� ��ġ�� �̵�

        if (!lockedTarget)
        {
            CameraTargetRotation(); // �Ϲ����� ī�޶� ȸ�� ����
           
        }
        else
        {
            
            LookAtTarget(); // Ÿ���� �ٶ󺸴� ī�޶� ȸ�� ����
        }

      
    }
    public float x;
    public float y;

    //bool toggle;
    //void FreeLook()
    //{
    //    rotY = 1.8f; // Y ȸ������ ���������� ���� (Ÿ���� �������� �ٶ�)
    //}

    void CameraTargetRotation()
    {
        // ���߿� �ʱ� ī�޶� �ֱ�

        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // ���콺 ������ ���
        rotX += (mouseAxis.x * senstivity) * Time.deltaTime; // X ȸ���� ����
        rotY -= ((mouseAxis.y * senstivity) * Time.deltaTime); // Y ȸ���� ����
       
        
        rotY = 100f; //���߿� �ּ�ó�� ���ֱ�


        rotY = Mathf.Clamp(rotY, 0,180); // Y ȸ������ �ּҿ� �ִ� �� ���̷� ���� (-clampAxis.x�� ����Ͽ� ���� ������ ����)

        Quaternion localRotation = Quaternion.Euler(rotY, rotX, 0); // X�� Y ȸ������ �̿��� ȸ�� Quaternion ����
        transform.rotation = Quaternion.Slerp(transform.rotation, localRotation, rotate_Smoothing); // �ε巯�� ȸ�� ����
        
    }

    [SerializeField] float PlusPoint =0;
    void LookAtTarget()
    {
        transform.rotation = cam.rotation; // Ÿ���� �ٶ󺸴� ī�޶� ȸ���� ���� ī�޶��� ȸ�������� ����
        Vector3 r = cam.eulerAngles; // ���� ī�޶��� ���� ����
        rotX = r.y; // X ȸ������ ���� ī�޶��� Y ������ ����
        rotY = 1.8f + PlusPoint; // Y ȸ������ ���������� ���� (Ÿ���� �������� �ٶ�)
       
    }
}