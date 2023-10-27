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
        if (Instance == null) //�������� �ڽ��� üũ��, null����
        {
            Instance = this; //���� �ڱ� �ڽ��� ������.
           
        }
    }

    [Header("SandWorm Info")]
    public float SandWormHP;
    public float SandWormMaxHP;
    public float SandWormSpeed;
    public float SandWormOriginSpeed;
    public float SandWormMaxSpeed;

    public float SandWormAttackDamage;
    public float SandWormDef; //����


    public bool IsBulling = false; //�÷��̾�� ���� �ް� �ִ���

    public bool IsAttacking = false; //����� ���� ��������?

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
        SandWormHP = SandWormMaxHP; //HP�ʱ�ȭ
        SandWormOriginSpeed = SandWormSpeed;
        Player = GameObject.FindGameObjectWithTag("Player");
        SetPosition = GameObject.FindGameObjectWithTag("EnemyUpPos").transform;
        UnderSetPosition = GameObject.FindGameObjectWithTag("EnemyUnderPos").transform;
        SandWormUIHP.GetComponent<Slider>().minValue = 0;// ü�� ������ �ʱ�ȭ
        SandWormUIHP.GetComponent<Slider>().maxValue = SandWormMaxHP; //ü�� ������ �ʱ�ȭ
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
        //Debug.Log("���� ����"+name);
        anime_Nav.SetBool(name, true);
    }
    public void StopPattern(string name)
    {
       // Debug.Log("���� ��" + name);

        anime_Nav.SetBool(name, false);
    }



    private IEnumerator currentMovement;


    public void Goto(GameObject Thing, Vector3 StartPos, Vector3 EndPos, float Speed)
    {
        if (currentMovement != null)
        {
            // ���� �̹� �̵� ���� ���, ������ �̵� �ڷ�ƾ�� ����
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
                // �̵��� �Ϸ�Ǹ� �ڷ�ƾ�� ����
                yield break;
            }

            yield return null;
        }
    }





    //���� ���� �Լ�
    public void MakingSinMovement(GameObject thing, float _sideVectorMagnitude, float _Speed, Vector3 StartPos, Vector3 WaveStart, Vector3 WaveEnd, Vector3 endpos, float StopTime, string circle, AutoMoverLoopingStyle loopstyle)
    {
        // WaveStart�� WaveEnd ������ �Ÿ� ���
        int many = (int)Vector3.Distance(WaveStart, WaveEnd);
        // AnchorPoint ���� �ʱ�ȭ
        int Count = many / 3;

        // 'circle' �Ķ���� ���� ���� Count ����
        if (circle == "circle")
        {
            Count = 1;
        }

        // AutoMover ������Ʈ �߰� �� ����
        wormAutoMover = thing.AddComponent<AutoMover>();
        wormAutoMover.RunOnStart = false;
        wormAutoMover.AnchorPointSpace = AutoMoverAnchorPointSpace.world;
        wormAutoMover.FaceForward = true;
        wormAutoMover.DynamicUpVector = false;

        // AnchorPoint �߰�
        wormAutoMover.AddAnchorPoint(thing.transform.position, new Vector3(0, 0, 0), thing.transform.localScale);
        wormAutoMover.AddAnchorPoint(StartPos, new Vector3(0, 0, 0), thing.transform.localScale);
        wormAutoMover.AddAnchorPoint(WaveStart, new Vector3(0, 0, 0), thing.transform.localScale);

        // Sin �Լ��� ����Ͽ� �¿�� �����̵��� ����
        for (int i = 1; i <= Count; i++)
        {
            // AnchorPoint ��ġ ���
            Vector3 position = (i * (((WaveEnd - WaveStart) / Count)) + WaveStart);

            // WaveEnd�� WaveStart ������ ���� ���� ���
            Vector3 directionToPlayer = WaveEnd - WaveStart;

            // ���� ���� ���
            Vector3 sideVector = new Vector3(-directionToPlayer.z, 0, directionToPlayer.x).normalized;

            // sideVector ũ�� ����
            float sideVectorMagnitude = _sideVectorMagnitude;
            sideVector *= sideVectorMagnitude;

            // ¦�� ��° AnchorPoint������ sideVector�� ���ϰ� Ȧ�� ��°������ ����
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

        // AutoMover �ӵ� ����
        wormAutoMover.Length = _Speed;

        // LoopingStyle ����
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

        // AutoMover�� �� ���� �����ϵ��� ����
        wormAutoMover.StopAfter = 1;
        wormAutoMover.CurveStyle = AutoMoverCurve.SplineThroughPoints;

        // ���� ������ AnchorPoint �߰�
        wormAutoMover.AddAnchorPoint(endpos, thing.transform.eulerAngles, thing.transform.localScale);

        // AutoMover ����
        wormAutoMover.StartMoving();

        // StopTime�� 0���� ũ�� ���� �ð� �� AutoMover �Ͻ� ����
        if (StopTime > 0)
        {
            StartCoroutine(StopMoving());
        }

        // AutoMover �Ͻ� ������ ���� �ڷ�ƾ
        IEnumerator StopMoving()
        {
            yield return new WaitForSeconds(StopTime);
            wormAutoMover.Pause();
        }
    }

}
