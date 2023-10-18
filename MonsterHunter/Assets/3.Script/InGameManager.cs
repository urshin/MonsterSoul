using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

    [SerializeField] TimelineAsset[] timelineAssets; //Ÿ�Ӷ��� ������
    [SerializeField] GameObject CutScene; //Director�� ����ִ� ��
    PlayableDirector Director; //���̷���
    [SerializeField] GameObject SandWormNavTarget;
    [SerializeField] GameObject BossTrigger;

    public GameObject player;
    public GameObject CarmeraTarget;
    void Start()
    {
        Director = CutScene.GetComponent<PlayableDirector>();

        Director.enabled = true;
        //������
        Director.playableAsset = timelineAssets[FindScene("Opening")];
        Debug.Log(FindScene("Opening"));
        Director.Play();
        player = GameObject.FindWithTag("Player");
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
        if (!GameManager.Instance.IsBossRoomEnter)
        {
            DrawRayToCheckEnterBoss();
            
        }
     
        
    }
   
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
              
                playSandWormOpening();
                break;
            }
        }
    }
    void playSandWormOpening()
    {
        Director.playableAsset = timelineAssets[FindScene("SandWormOpening")];
        Director.Play();
        
    }

  

}
