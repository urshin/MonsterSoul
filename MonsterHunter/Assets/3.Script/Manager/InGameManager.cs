using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

    [SerializeField] TimelineAsset[] timelineAssets; // Ÿ�Ӷ��� �ּ� �迭
    [SerializeField] GameObject CutScene; // ���͸� ������ ���� ������Ʈ
    PlayableDirector Director; // �÷��̾�� ����
    [SerializeField] GameObject SandWormNavTarget; // ���� ���� �׺���̼� Ÿ��
    [SerializeField] GameObject BossTrigger; // ���� Ʈ����

    public GameObject player; // �÷��̾� ���� ������Ʈ
    public GameObject CameraTarget; // ī�޶� Ÿ�� ���� ������Ʈ

    public bool isCutScene; // ���� �ƾ� �� ����

    public bool IsStore; // ���� �� ����
    public GameObject PausePopUp;

    void Start()
    {
        Director = CutScene.GetComponent<PlayableDirector>();

        Director.enabled = true;
        // ������ ���
        Invoke("PlayOpening", 0.2f);
        player = GameObject.FindWithTag("Player");
        SoundManager.Instance.PlayBGM("BGM_InGame");
    }

    int FindScene(string name) // Ÿ�Ӷ��� �ּ� �迭���� �̸��� �ش��ϴ� �ּ��� �ε����� ã�� �Լ�
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
            TheWorld(); // ���� �ð� �Ͻ� ���� �Լ� ȣ��
        }
    }

    bool Timestop = false;

    public void TheWorld()
    {
        Timestop = !Timestop; // ���� �ð� �Ͻ� ���� ���
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

        Vector3 halfExtents = BossTrigger.GetComponent<BoxCollider>().size; // �ڽ� �ݶ��̴� ũ�� ��������
        Quaternion orientation = Quaternion.identity;

        Collider[] colliders = Physics.OverlapBox(center, halfExtents, orientation);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                GameManager.Instance.IsBossRoomEnter = true;
                playSandWormOpening(); // ���� �� ������ ���
                Invoke("ShowSandWormUI", 9.5f);
                break;
            }
        }
    }

    [SerializeField] GameObject ShowSandWormUIHP;

    void ShowSandWormUI()
    {
        StartCoroutine(UIMoving(new Vector3(0, 10, 0), 10)); // UI �̵� �ִϸ��̼� ����
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
        Director.playableAsset = timelineAssets[FindScene("SandWormOpening")]; // ���� �� ������ Ÿ�Ӷ��� ���
        Director.Play();
    }

    void PlayOpening()
    {
        Director.playableAsset = timelineAssets[FindScene("Opening")]; // ���� ������ Ÿ�Ӷ��� ���
        Director.Play();
    }
}