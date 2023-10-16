using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class InGameManager : MonoBehaviour
{
    [SerializeField] TimelineAsset[] timelineAssets; //Ÿ�Ӷ��� ������
    [SerializeField] GameObject CutScene; //Director�� ����ִ� ��
    PlayableDirector Director; //���̷���
    [SerializeField] GameObject SandWormNavTarget;
    [SerializeField] GameObject BossTrigger;



    void Start()
    {
        Director = CutScene.GetComponent<PlayableDirector>();

        Director.enabled = true;
        //������
        Director.playableAsset = timelineAssets[FindScene("Opening")];
        Debug.Log(FindScene("Opening"));
        Director.Play();
    }

    int FindScene(string name) //timelineAssets���� ��°�� �˷���
    {
        for (int i = 0; i < timelineAssets.Length; i++)
        {
            if (timelineAssets[i].name.Contains(name))
            {
                return i;
            }
        }
        return -1;
    }





    // Update is called once per frame
    void Update()
    {
        if (!enterCheck)
        {
            DrawRayToCheckEnterBoss();
        }
        
    }
    bool enterCheck = false;
    private void DrawRayToCheckEnterBoss()
    {
        Vector3 center = BossTrigger.transform.position;

        Vector3 halfExtents = BossTrigger.GetComponent<BoxCollider>().size; ; // �ڽ��� ũ�⸦ �����մϴ�.
        Quaternion orientation = Quaternion.identity;

        Collider[] colliders = Physics.OverlapBox(center, halfExtents, orientation);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log("������ ����");
                GameManager.Instance.IsBossRoomEnter = true;
                enterCheck = true;
                playSandWormOpening();
                break;
            }
        }
    }
    void playSandWormOpening()
    {
        Director.playableAsset = timelineAssets[FindScene("SandWormOpening")];
        Director.Play();
        SandWormNavTarget.GetComponent<AutoMoverTest>().enabled= true;
    }


}
