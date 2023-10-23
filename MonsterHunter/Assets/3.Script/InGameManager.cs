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

    public bool isCutScene;

    public bool IsStore;
    void Start()
    {
        Director = CutScene.GetComponent<PlayableDirector>();

        Director.enabled = true;
        //������
        Invoke("PlayOpening", 0.2f);
        //PlayOpening();
        player = GameObject.FindWithTag("Player");
        SoundManager.Instance.PlayBGM("BGM_InGame");
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
               // Debug.Log("������ ����");
                GameManager.Instance.IsBossRoomEnter = true;
              
                playSandWormOpening();
                Invoke("ShowSandWormUI", 9.5f);
                break;
            }
        }
    }
    [SerializeField] GameObject ShowSandWormUIHP;
    void ShowSandWormUI()
    {
        StartCoroutine(UIMoving(new Vector3(0, 10, 0), 10));
    }
    IEnumerator UIMoving(Vector3 Howmuch, float Smooth)
    {
        for (int i = 0; i < Smooth; i++)
        {
            ShowSandWormUIHP.GetComponent<RectTransform>().localPosition += Howmuch;
            yield return new WaitForSecondsRealtime(0.01f);

        }
    
    }


    void playSandWormOpening()
    {
        Director.playableAsset = timelineAssets[FindScene("SandWormOpening")];
        Director.Play();
        
    }
    void PlayOpening()
    {
        Director.playableAsset = timelineAssets[FindScene("Opening")];
        Director.Play();

    }


}
