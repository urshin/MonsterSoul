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
    //몇초에 한번씩 플레이어를 찾고 해당 방향으로 브레스(아님 뭔가 던지기)
    GameObject SandWormBody;
    [SerializeField] GameObject head;

    [SerializeField] GameObject[] Rock;

    float radius;
    private float timerDuration = 2.5f; // 타이머의 기간(초)
    private float timeRemaining; // 남은 시간(초)
    private float lastTimeChecked; // 마지막으로 시간을 체크한 시간
    private float outputInterval = 0.3f; // 출력 간격(초)
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
                enabled = false; // 업데이트를 중지합니다.
            }
        }


       
       



    }







}























//사인 움직임
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
//    // Sin 함수를 사용하여 X 좌표를 조정하여 양옆으로 움직임


//    for (int i = 1; i <= _HowMany; i++)
//    {

//        Vector3 position = (i * (((playerObject.transform.position - gameObject.transform.position) / (_HowMany+1f))) + transform.position);


//        // 이 오브젝트가 플레이어 오브젝트를 바라보는 방향을 계산합니다.
//        Vector3 directionToPlayer = playerObject.transform.position - transform.position;

//        // 수직 벡터를 계산합니다. 이를 위해 x와 z 구성 요소를 교환하고 그 중 하나를 부호를 바꾸어 사용합니다.
//        Vector3 sideVector = new Vector3(-directionToPlayer.z, 0, directionToPlayer.x).normalized;

//        // 필요한 경우 sideVector의 크기를 조절할 수 있습니다.
//        float sideVectorMagnitude = _sideVectorMagnitude; // 필요한 크기에 맞게 이 값을 조절하세요.
//        sideVector *= sideVectorMagnitude;

//        if (i % 2 == 0)
//        {
//            // 이제 sideVector를 위치에 추가할 수 있습니다.
//            position += sideVector;

//        }
//        else
//        {
//            position -= sideVector;
//        }

//        // 나머지 코드...


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

