using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

    [SerializeField] TimelineAsset[] timelineAssets; // 타임라인 애셋 배열
    [SerializeField] GameObject CutScene; // 디렉터를 포함한 게임 오브젝트
    PlayableDirector Director; // 플레이어블 디렉터
    [SerializeField] GameObject SandWormNavTarget; // 샌드 웜의 네비게이션 타겟
    [SerializeField] GameObject BossTrigger; // 보스 트리거

    public GameObject player; // 플레이어 게임 오브젝트
    public GameObject CameraTarget; // 카메라 타겟 게임 오브젝트

    public bool isCutScene; // 현재 컷씬 중 여부

    public bool IsStore; // 상점 중 여부
    public GameObject PausePopUp;

    void Start()
    {
        Director = CutScene.GetComponent<PlayableDirector>();

        Director.enabled = true;
        // 오프닝 재생
        Invoke("PlayOpening", 0.2f);
        player = GameObject.FindWithTag("Player");
        SoundManager.Instance.PlayBGM("BGM_InGame");
    }

    int FindScene(string name) // 타임라인 애셋 배열에서 이름에 해당하는 애셋의 인덱스를 찾는 함수
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

    void Update()
    {
        if (!GameManager.Instance.IsBossRoomEnter)
        {
            DrawRayToCheckEnterBoss();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TheWorld(); // 게임 시간 일시 정지 함수 호출
        }
    }

    bool Timestop = false;

    public void TheWorld()
    {
        Timestop = !Timestop; // 게임 시간 일시 정지 토글
        if (Timestop)
        {
            PausePopUp.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            PausePopUp.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void DrawRayToCheckEnterBoss()
    {
        Vector3 center = BossTrigger.transform.position;

        Vector3 halfExtents = BossTrigger.GetComponent<BoxCollider>().size; // 박스 콜라이더 크기 가져오기
        Quaternion orientation = Quaternion.identity;

        Collider[] colliders = Physics.OverlapBox(center, halfExtents, orientation);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                GameManager.Instance.IsBossRoomEnter = true;
                playSandWormOpening(); // 샌드 웜 오프닝 재생
                Invoke("ShowSandWormUI", 9.5f);
                break;
            }
        }
    }

    [SerializeField] GameObject ShowSandWormUIHP;

    void ShowSandWormUI()
    {
        StartCoroutine(UIMoving(new Vector3(0, 10, 0), 10)); // UI 이동 애니메이션 시작
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
        Director.playableAsset = timelineAssets[FindScene("SandWormOpening")]; // 샌드 웜 오프닝 타임라인 재생
        Director.Play();
    }

    void PlayOpening()
    {
        Director.playableAsset = timelineAssets[FindScene("Opening")]; // 게임 오프닝 타임라인 재생
        Director.Play();
    }
}