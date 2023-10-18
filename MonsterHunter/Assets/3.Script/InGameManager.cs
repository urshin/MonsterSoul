using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

    [SerializeField] TimelineAsset[] timelineAssets; //타임라인 모음집
    [SerializeField] GameObject CutScene; //Director가 들어있는 곳
    PlayableDirector Director; //다이렉터
    [SerializeField] GameObject SandWormNavTarget;
    [SerializeField] GameObject BossTrigger;

    public GameObject player;
    public GameObject CarmeraTarget;
    void Start()
    {
        Director = CutScene.GetComponent<PlayableDirector>();

        Director.enabled = true;
        //오프닝
        Director.playableAsset = timelineAssets[FindScene("Opening")];
        Debug.Log(FindScene("Opening"));
        Director.Play();
        player = GameObject.FindWithTag("Player");
    }

    int FindScene(string name) //timelineAssets내의 번째를 알려줌
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

        Vector3 halfExtents = BossTrigger.GetComponent<BoxCollider>().size; ; // 박스의 크기를 조절합니다.
        Quaternion orientation = Quaternion.identity;

        Collider[] colliders = Physics.OverlapBox(center, halfExtents, orientation);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log("보스룸 입장");
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
