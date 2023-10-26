using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    public void Awake()
    {
        if (Instance == null) // 플레이어의 인스턴스가 아직 생성되지 않았을 때
        {
            Instance = this; // 현재 스크립트의 인스턴스를 할당하여 싱글톤 패턴을 구현
        }
    }
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
    public GameObject BossEnding;
    public GameObject PlayerEnding;

    public float TimeSpend;
    void Start()
    {
        TimeSpend = 0f; //시간 초기화
        Director = CutScene.GetComponent<PlayableDirector>();
        GameManager.Instance.cursorLocked = false;

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
            TheWorld(PausePopUp); // 게임 시간 일시 정지 함수 호출
        }
    }

    private void FixedUpdate()
    {
        TimeSpend += Time.deltaTime;
    }

    private float targetTimeScale = 0.1f; // 목표 타임 스케일 값
    private float timeScaleChangeRate = 0.01f; // 매 프레임마다 변경될 타임 스케일 값
    private float minTimeScale = 0.0f; // 시간이 멈출 최소 타임 스케일 값

    public void SlowDowntime(GameObject pop)
    {
        StartCoroutine(SlowDownTime(pop));
        Debug.Log("시간 줄기 on");
        
    }
    IEnumerator SlowDownTime( GameObject pop)
    {
        Debug.Log("시간 줄기 진입");
        while (Time.timeScale > minTimeScale)
        {
            Time.timeScale -= timeScaleChangeRate;

            if (Time.timeScale < targetTimeScale)
            {
                Time.timeScale = minTimeScale;
                break;
            }

            yield return null;
        }

        pop.SetActive(true);
        // 시간이 멈춘 후에 원하는 작업을 수행할 수 있습니다.
        GameManager.Instance.cursorLocked = true;
        pop.transform.GetChild(0).transform.Find("Time").gameObject.GetComponent<Text>().text = "Time:" + TimeSpend.ToString();
        yield return null;
    }



    bool Timestop = false;

    public void TheWorld(GameObject pop)
    {
        Timestop = !Timestop; // 게임 시간 일시 정지 토글
        if (Timestop)
        {
            pop.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pop.SetActive(false);
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