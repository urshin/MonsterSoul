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
    public float SandWormHP;
    public float SandWormMaxHP;
    public float SandWormSpeed;

    public bool changeState = true;


    public GameObject Player;


    public bool IsBulling = false;

    public Animator anime_Nav;
   
    public enum SandWormState 
    {
        Opening,
        Ingage,
        Attack1,
        Attack2,
        Rage,
        Die,
    }
    public SandWormState currentSandWormState;
    
    

    void Start()
    {
        currentSandWormState = SandWormState.Opening;
        SandWormHP = SandWormMaxHP; //HP�ʱ�ȭ
        Player = GameObject.FindGameObjectWithTag("Player");
       

    }
 

    void Update()
    {
        anime_Nav.SetFloat("HP", (SandWormHP/SandWormMaxHP) *100);
       

    }

    public void StartPattern(string name)
    {
        Debug.Log(name);
        anime_Nav.SetBool(name, true);
    }
    public void StopPattern(string name)
    {
        anime_Nav.SetBool(name, false);
    }


}
