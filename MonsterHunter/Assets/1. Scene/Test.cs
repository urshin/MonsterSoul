using AutoMoverPro;
using Cinemachine;
using DistantLands;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Test : MonoBehaviour
{
    //���ʿ� �ѹ��� �÷��̾ ã�� �ش� �������� �극��(�ƴ� ���� ������)
    GameObject SandWormBody;
    [SerializeField] GameObject head;

    [SerializeField] GameObject[] Rock;

    float radius;
    private float timerDuration = 2.5f; // Ÿ�̸��� �Ⱓ(��)
    private float timeRemaining; // ���� �ð�(��)
    private float lastTimeChecked; // ���������� �ð��� üũ�� �ð�
    private float outputInterval = 0.3f; // ��� ����(��)
    void Start()
    {
        radius = 24;
        SandWormBoss.Instance.MakingSinMovement(gameObject, 1, 4,transform.position + new Vector3(radius*2, 0, 0), transform.position + new Vector3(0, radius, 0), transform.position + new Vector3(-radius*2, 0, 0), transform.position + new Vector3(0, -100, 0), 3.8f,"circle",AutoMoverLoopingStyle.repeat);
        OktoShootingFire = true;


        timeRemaining = timerDuration;
        lastTimeChecked = Time.time;


    }
    bool OktoShootingFire;
    float timer = 0;    
    void ShootingFire()
    {
        for(int i = 0; i < Rock.Length; i++)
        {
            GameObject rock = Instantiate(Rock[i], transform.position, Quaternion.identity);
            Destroy(rock,5f);
        }
    }
    

    private void Update()
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
                OktoShootingFire = false;
                enabled = false; // ������Ʈ�� �����մϴ�.
            }
        }


       
       



    }







}























//���� ������
//GameObject playerObject;
//AutoMover worm;
//private void Start()
//{

//    playerObject = GameObject.FindGameObjectWithTag("Player");


//    worm = gameObject.AddComponent<AutoMover>();

//    MakingSinMovement(10, 10);
//}

//private void MakingSinMovement(float _sideVectorMagnitude, int _HowMany)
//{
//    worm.RunOnStart = false;

//    worm.AnchorPointSpace = AutoMoverAnchorPointSpace.world;

//    worm.FaceForward = true;
//    worm.DynamicUpVector = false;
//    worm.AddAnchorPoint(transform.position + new Vector3(0, 0, 0), new Vector3(0, 0, 0), transform.localScale);
//    // Sin �Լ��� ����Ͽ� X ��ǥ�� �����Ͽ� �翷���� ������


//    for (int i = 1; i <= _HowMany; i++)
//    {

//        Vector3 position = (i * (((playerObject.transform.position - gameObject.transform.position) / (_HowMany+1f))) + transform.position);


//        // �� ������Ʈ�� �÷��̾� ������Ʈ�� �ٶ󺸴� ������ ����մϴ�.
//        Vector3 directionToPlayer = playerObject.transform.position - transform.position;

//        // ���� ���͸� ����մϴ�. �̸� ���� x�� z ���� ��Ҹ� ��ȯ�ϰ� �� �� �ϳ��� ��ȣ�� �ٲپ� ����մϴ�.
//        Vector3 sideVector = new Vector3(-directionToPlayer.z, 0, directionToPlayer.x).normalized;

//        // �ʿ��� ��� sideVector�� ũ�⸦ ������ �� �ֽ��ϴ�.
//        float sideVectorMagnitude = _sideVectorMagnitude; // �ʿ��� ũ�⿡ �°� �� ���� �����ϼ���.
//        sideVector *= sideVectorMagnitude;

//        if (i % 2 == 0)
//        {
//            // ���� sideVector�� ��ġ�� �߰��� �� �ֽ��ϴ�.
//            position += sideVector;

//        }
//        else
//        {
//            position -= sideVector;
//        }

//        // ������ �ڵ�...


//        Vector3 rotation = playerObject.transform.eulerAngles;
//        Vector3 scale = gameObject.transform.localScale;
//        worm.AddAnchorPoint(position, rotation, scale);
//    }
//    worm.Length = 10f;
//    worm.LoopingStyle = AutoMoverLoopingStyle.repeat;
//    worm.CurveStyle = AutoMoverCurve.SplineThroughPoints;


//    worm.AddAnchorPoint(playerObject.transform.position, playerObject.transform.eulerAngles, playerObject.transform.localScale);



//    worm.Length = 10f;
//    worm.LoopingStyle = AutoMoverLoopingStyle.repeat;
//    worm.CurveStyle = AutoMoverCurve.SplineThroughPoints;

//    worm.StartMoving();
//}

