using AutoMoverPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static SandWormBoss;

public class SandWormMovement : MonoBehaviour
{

   // public SandWorm sandWorm; //»÷µå¿ú ÂüÁ¶

    Animator anime;

    [SerializeField] GameObject position;

    [SerializeField] List<Transform> PositionAnchor = new List<Transform>();

    AutoMover CurrentAutoMover;

    bool BossEnter = true;

    private void Start()
    {
        Destroy(gameObject.GetComponent<AutoMover>());
        PositionAnchor = new List<Transform>();
        anime = GetComponent<Animator>();
    }








    private void MakeAutoMove(GameObject Position, float Speed)
    {
       // sandWorm.currentSandWormState = SandWorm.SandWormState.Opening;
        PositionAnchor.Clear();
        PositionAnchor = new List<Transform>();
        //Debug.Log(Position.transform.childCount);
        for (int i = 0; i < Position.transform.childCount; i++)
        {
            PositionAnchor.Add(Position.transform.GetChild(i).gameObject.transform);

        }
        AutoMover WormMove = gameObject.AddComponent<AutoMover>();
        CurrentAutoMover = WormMove;
        WormMove.RunOnStart = false;

        WormMove.AnchorPointSpace = AutoMoverAnchorPointSpace.world;

        WormMove.FaceForward = false;
        WormMove.DynamicUpVector = true;

        for (int i = 0; i < Position.transform.childCount; i++)
        {
            WormMove.AddAnchorPoint(PositionAnchor[i].transform.position, transform.localScale);

        }

        WormMove.Length = Speed;
        WormMove.LoopingStyle = AutoMoverLoopingStyle.repeat;
        WormMove.CurveStyle = AutoMoverCurve.SplineThroughPoints;
        // WormMove.StopAfter = 1;

        WormMove.StartMoving();
        Invoke("PauseAutoMoving", Speed - 0.1f);
    }



    void PauseAutoMoving()
    {
        CurrentAutoMover.Pause();
        Invoke("Ingage", 3f);
        
    }
    void Ingage()
    {
       SandWormBoss.Instance.StartPattern("Ingage");
        
        SandWormBoss.Instance.StartPattern("InGame");
        Destroy(CurrentAutoMover);
    }

    

    private void Update()
    {
        if (GameManager.Instance.IsBossRoomEnter)
        {
            if (BossEnter)
            {
                MakeAutoMove(position, 10);

                BossEnter = false;
            }

            if (SandWormBoss.Instance. SandWormHP <= 0)
            {
                anime.SetBool("IsDead", true);


            }



        }

    }


    



}
