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
        
        SandWormHP = SandWormMaxHP; //HP�ʱ�ȭ
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
        Debug.Log("���� ����"+name);
        anime_Nav.SetBool(name, true);
    }
    public void StopPattern(string name)
    {
        Debug.Log("���� ��" + name);

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
        // Sin �Լ��� ����Ͽ� X ��ǥ�� �����Ͽ� �翷���� ������


        for (int i = 1; i <= Count; i++)
        {

            Vector3 position = (i * (((WaveEnd - WaveStart) / Count)) + WaveStart);


            // �� ������Ʈ�� �÷��̾� ������Ʈ�� �ٶ󺸴� ������ ����մϴ�.
            Vector3 directionToPlayer = WaveEnd - WaveStart;

            // ���� ���͸� ����մϴ�. �̸� ���� x�� z ���� ��Ҹ� ��ȯ�ϰ� �� �� �ϳ��� ��ȣ�� �ٲپ� ����մϴ�.
            Vector3 sideVector = new Vector3(-directionToPlayer.z, 0, directionToPlayer.x).normalized;


            // �ʿ��� ��� sideVector�� ũ�⸦ ������ �� �ֽ��ϴ�.
            float sideVectorMagnitude = _sideVectorMagnitude; // �ʿ��� ũ�⿡ �°� �� ���� �����ϼ���.
            sideVector *= sideVectorMagnitude;

            if (i % 2 == 0)
            {
                // ���� sideVector�� ��ġ�� �߰��� �� �ֽ��ϴ�.
                position += sideVector;

            }
            else
            {
                position -= sideVector;
            }

            // ������ �ڵ�...


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
        
        wormAutoMover.StopAfter =1; //�ѹ��� �����ϰ� ���߰���
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
