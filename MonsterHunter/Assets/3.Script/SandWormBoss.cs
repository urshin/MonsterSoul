using AutoMoverPro;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


public class SandWormBoss : MonoBehaviour
{
    public static SandWormBoss Instance;
    public void Awake()
    {
        if (Instance == null) //정적으로 자신을 체크함, null인진
        {
            Instance = this; //이후 자기 자신을 저장함.
           
        }
    }

    [Header("SandWorm Info")]
    public float SandWormHP;
    public float SandWormMaxHP;
    public float SandWormSpeed;
    public float SandWormOriginSpeed;
    public float SandWormMaxSpeed;

    public float SandWormAttackDamage;
    public float SandWormDef; //방어력


    public bool IsBulling = false; //플레이어에게 공격 받고 있는지

    public bool IsAttacking = false; //샌드웜 공격 상태인지?

    [Header("GetInfo")]
    public GameObject Player;
    public Animator anime_Nav;
     public Transform UnderSetPosition;
     public Transform SetPosition;
    public Transform SandWormLastPosition;
    public AutoMover wormAutoMover;

    //public enum SandWormState 
    //{
    //    Opening,
    //    Ingage,
    //    Attack1,
    //    Attack2,
    //    Rage,
    //    Die,
    //}
    //public SandWormState currentSandWormState;



    void Start()
    {
        
        SandWormHP = SandWormMaxHP; //HP초기화
        SandWormOriginSpeed = SandWormSpeed;
        Player = GameObject.FindGameObjectWithTag("Player");
        SetPosition = GameObject.FindGameObjectWithTag("EnemyUpPos").transform;
        UnderSetPosition = GameObject.FindGameObjectWithTag("EnemyUnderPos").transform;

    }
 

    void Update()
    {
        anime_Nav.SetFloat("HP", (SandWormHP/SandWormMaxHP) *100);
       

    }

    public void StartPattern(string name)
    {
        Debug.Log("패턴 시작"+name);
        anime_Nav.SetBool(name, true);
    }
    public void StopPattern(string name)
    {
        Debug.Log("패턴 끝" + name);

        anime_Nav.SetBool(name, false);
    }



    private IEnumerator currentMovement;


    public void Goto(GameObject Thing, Vector3 StartPos, Vector3 EndPos, float Speed)
    {
        if (currentMovement != null)
        {
            // 만약 이미 이동 중인 경우, 현재의 이동 코루틴을 중지
            StopCoroutine(currentMovement);
        }
        currentMovement = MoveObject(Thing, StartPos, EndPos, Speed);
        StartCoroutine(currentMovement);
    }

    public IEnumerator MoveObject(GameObject Thing, Vector3 StartPos, Vector3 EndPos, float Speed)
    {
        float journeyLength = Vector3.Distance(StartPos, EndPos);
        float startTime = Time.time;

        while (true)
        {
            float distanceCovered = (Time.time - startTime) * Speed;
            float fractionOfJourney = distanceCovered / journeyLength;

            Thing.transform.position = Vector3.Lerp(StartPos, EndPos, fractionOfJourney);

            if (fractionOfJourney >= 1)
            {
                // 이동이 완료되면 코루틴을 종료
                yield break;
            }

            yield return null;
        }
    }





    //오토 무브 함수
    public void MakingSinMovement(GameObject thing, float _sideVectorMagnitude, float _Speed, Vector3 StartPos, Vector3 WaveStart, Vector3 WaveEnd, Vector3 endpos, float StopTime, string circle, AutoMoverLoopingStyle loopstyle)
    {

        int many = (int)Vector3.Distance(WaveStart, WaveEnd);
        int Count = many / 3;
        if(circle == "circle")
        {
            Count = 1;
        }
        wormAutoMover = thing.AddComponent<AutoMover>();
        wormAutoMover.RunOnStart = false;

        wormAutoMover.AnchorPointSpace = AutoMoverAnchorPointSpace.world;

        wormAutoMover.FaceForward = true;
        wormAutoMover.DynamicUpVector = false;
        wormAutoMover.AddAnchorPoint(thing.transform.position, new Vector3(0, 0, 0), thing.transform.transform.localScale);
        wormAutoMover.AddAnchorPoint(StartPos, new Vector3(0, 0, 0), thing.transform.transform.localScale);
        wormAutoMover.AddAnchorPoint(WaveStart, new Vector3(0, 0, 0), thing.transform.transform.localScale);
        // Sin 함수를 사용하여 X 좌표를 조정하여 양옆으로 움직임


        for (int i = 1; i <= Count; i++)
        {

            Vector3 position = (i * (((WaveEnd - WaveStart) / Count)) + WaveStart);


            // 이 오브젝트가 플레이어 오브젝트를 바라보는 방향을 계산합니다.
            Vector3 directionToPlayer = WaveEnd - WaveStart;

            // 수직 벡터를 계산합니다. 이를 위해 x와 z 구성 요소를 교환하고 그 중 하나를 부호를 바꾸어 사용합니다.
            Vector3 sideVector = new Vector3(-directionToPlayer.z, 0, directionToPlayer.x).normalized;


            // 필요한 경우 sideVector의 크기를 조절할 수 있습니다.
            float sideVectorMagnitude = _sideVectorMagnitude; // 필요한 크기에 맞게 이 값을 조절하세요.
            sideVector *= sideVectorMagnitude;

            if (i % 2 == 0)
            {
                // 이제 sideVector를 위치에 추가할 수 있습니다.
                position += sideVector;

            }
            else
            {
                position -= sideVector;
            }

            // 나머지 코드...


            Vector3 rotation = thing.transform.eulerAngles;
            Vector3 scale = thing.transform.transform.localScale;
            wormAutoMover.AddAnchorPoint(position, rotation, scale);
        }
        wormAutoMover.Length = _Speed;
        if(loopstyle == AutoMoverLoopingStyle.bounce)
        {
        wormAutoMover.LoopingStyle = AutoMoverLoopingStyle.bounce;

        }
        if(loopstyle == AutoMoverLoopingStyle.repeat)
        {
        wormAutoMover.LoopingStyle = AutoMoverLoopingStyle.repeat;

        }
        if(loopstyle == AutoMoverLoopingStyle.loop)
        {
        wormAutoMover.LoopingStyle = AutoMoverLoopingStyle.loop;

        }
        
        wormAutoMover.StopAfter =1; //한번만 실행하고 멈추게함
        wormAutoMover.CurveStyle = AutoMoverCurve.SplineThroughPoints;


        wormAutoMover.AddAnchorPoint(endpos, thing.transform.eulerAngles, thing.transform.localScale);

        wormAutoMover.StartMoving();

        if(StopTime > 0)
        {

        StartCoroutine(StopMoving());
        }

        IEnumerator StopMoving()
        {
            yield return new WaitForSeconds(StopTime);
            wormAutoMover.Pause();
        }
    }


}
