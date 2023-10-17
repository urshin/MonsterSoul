using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


public class SandWorm : MonoBehaviour
{
   
    float SandWormHP;
    public float SandWormMaxHP;
    public float SandWormSpeed;

    public bool changeState = true;


    public GameObject Player;
   
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
        SandWormHP = SandWormMaxHP; //HP√ ±‚»≠
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    
    void Update()
    {

       

    }
}
