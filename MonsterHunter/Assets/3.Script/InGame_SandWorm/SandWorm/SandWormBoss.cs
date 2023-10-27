using AutoMoverPro;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

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

    [SerializeField] GameObject SandWormUIHP;


    [Header("GetInfo")]
    public GameObject Player;
    public Animator anime_Nav;
     public Transform UnderSetPosition;
     public Transform SetPosition;
    public Transform SandWormLastPosition;
    public AutoMover wormAutoMover;

    GameManager GM;
    bool IsBossDead = false;
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
        GM = GameManager.Instance;
        GM.SandWormLoad = false;
        SandWormHP = SandWormMaxHP; //HP초기화
        SandWormOriginSpeed = SandWormSpeed;
        Player = GameObject.FindGameObjectWithTag("Player");
        SetPosition = GameObject.FindGameObjectWithTag("EnemyUpPos").transform;
        UnderSetPosition = GameObject.FindGameObjectWithTag("EnemyUnderPos").transform;
        SandWormUIHP.GetComponent<Slider>().minValue = 0;// 체력 게이지 초기화
        SandWormUIHP.GetComponent<Slider>().maxValue = SandWormMaxHP; //체력 게이지 초기화
        IsBossDead = false;
        Invoke("LateStart", 0.1f);
    }

    void LateStart()
    {
       GM.SandWormLoad = true;
    }

    void Update()
    {
        anime_Nav.SetFloat("HP", (SandWormHP/SandWormMaxHP) *100);

        SandWormUIHP.GetComponent<Slider>().value = SandWormHP;

        if(SandWormHP <= 0&&!IsBossDead)
        {

         //   InGameManager.Instance.SlowDowntime(InGameManager.Instance.BossEnding);
            //InGameManager.Instance.SlowDowntime();
            IsBossDead=true;
        }
    }
    
    public void StartPattern(string name)
    {
        //Debug.Log("패턴 시작"+name);
        anime_Nav.SetBool(name, true);
    }
    public void StopPattern(string name)
    {
       // Debug.Log("패턴 끝" + name);

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
        // WaveStart와 WaveEnd 사이의 거리 계산
        int many = (int)Vector3.Distance(WaveStart, WaveEnd);
        // AnchorPoint 개수 초기화
        int Count = many / 3;

        // 'circle' 파라미터 값에 따라 Count 설정
        if (circle == "circle")
        {
            Count = 1;
        }

        // AutoMover 컴포넌트 추가 및 설정
        wormAutoMover = thing.AddComponent<AutoMover>();
        wormAutoMover.RunOnStart = false;
        wormAutoMover.AnchorPointSpace = AutoMoverAnchorPointSpace.world;
        wormAutoMover.FaceForward = true;
        wormAutoMover.DynamicUpVector = false;

        // AnchorPoint 추가
        wormAutoMover.AddAnchorPoint(thing.transform.position, new Vector3(0, 0, 0), thing.transform.localScale);
        wormAutoMover.AddAnchorPoint(StartPos, new Vector3(0, 0, 0), thing.transform.localScale);
        wormAutoMover.AddAnchorPoint(WaveStart, new Vector3(0, 0, 0), thing.transform.localScale);

        // Sin 함수를 사용하여 좌우로 움직이도록 설정
        for (int i = 1; i <= Count; i++)
        {
            // AnchorPoint 위치 계산
            Vector3 position = (i * (((WaveEnd - WaveStart) / Count)) + WaveStart);

            // WaveEnd와 WaveStart 사이의 방향 벡터 계산
            Vector3 directionToPlayer = WaveEnd - WaveStart;

            // 수직 벡터 계산
            Vector3 sideVector = new Vector3(-directionToPlayer.z, 0, directionToPlayer.x).normalized;

            // sideVector 크기 설정
            float sideVectorMagnitude = _sideVectorMagnitude;
            sideVector *= sideVectorMagnitude;

            // 짝수 번째 AnchorPoint에서는 sideVector를 더하고 홀수 번째에서는 빼기
            if (i % 2 == 0)
            {
                position += sideVector;
            }
            else
            {
                position -= sideVector;
            }

            Vector3 rotation = thing.transform.eulerAngles;
            Vector3 scale = thing.transform.localScale;
            wormAutoMover.AddAnchorPoint(position, rotation, scale);
        }

        // AutoMover 속도 설정
        wormAutoMover.Length = _Speed;

        // LoopingStyle 설정
        if (loopstyle == AutoMoverLoopingStyle.bounce)
        {
            wormAutoMover.LoopingStyle = AutoMoverLoopingStyle.bounce;
        }
        else if (loopstyle == AutoMoverLoopingStyle.repeat)
        {
            wormAutoMover.LoopingStyle = AutoMoverLoopingStyle.repeat;
        }
        else if (loopstyle == AutoMoverLoopingStyle.loop)
        {
            wormAutoMover.LoopingStyle = AutoMoverLoopingStyle.loop;
        }

        // AutoMover가 한 번만 실행하도록 설정
        wormAutoMover.StopAfter = 1;
        wormAutoMover.CurveStyle = AutoMoverCurve.SplineThroughPoints;

        // 종료 지점에 AnchorPoint 추가
        wormAutoMover.AddAnchorPoint(endpos, thing.transform.eulerAngles, thing.transform.localScale);

        // AutoMover 시작
        wormAutoMover.StartMoving();

        // StopTime이 0보다 크면 일정 시간 후 AutoMover 일시 정지
        if (StopTime > 0)
        {
            StartCoroutine(StopMoving());
        }

        // AutoMover 일시 정지를 위한 코루틴
        IEnumerator StopMoving()
        {
            yield return new WaitForSeconds(StopTime);
            wormAutoMover.Pause();
        }
    }

}
