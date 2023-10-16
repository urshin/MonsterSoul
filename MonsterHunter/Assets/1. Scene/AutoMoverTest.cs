using AutoMoverPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AutoMoverTest : MonoBehaviour
{
    [SerializeField] GameObject position1;

    [SerializeField] List<Transform> PositionAnchor = new List<Transform>();
    AutoMover WormMove;
    private void Start()
    {

    }

    private void MakeAutoMove(GameObject Position, float Speed)
    {
        Debug.Log(Position.transform.childCount);
        for (int i = 0; i < Position.transform.childCount; i++)
        {
            PositionAnchor.Add(Position.transform.GetChild(i).gameObject.transform);

        }
        WormMove = gameObject.AddComponent<AutoMover>();
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
        WormMove.StopAfter = 1;


        WormMove.StartMoving();
        ReadPosition = false;

    }


    bool ReadPosition = true;
    float time;
    private void Update()
    {
        if (GameManager.Instance.IsBossRoomEnter)
        {
            if (ReadPosition)
            {
                MakeAutoMove(position1, 10);
                ReadPosition = false;

            }



            time += Time.deltaTime;
            Debug.Log(time);
            if (WormMove.Length - 0.1 <= time)
            {
                WormMove.Pause();
                GameManager.Instance.IsBossRoomEnter = false;
            }
        }
    }


}
