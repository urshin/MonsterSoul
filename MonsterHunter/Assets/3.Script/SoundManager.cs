using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip[] EffectClip;
    public AudioClip[] BGMClip;

    public AudioSource Bgm;
    public GameObject EffectPos;
    public GameObject Effect;
    public void Awake()
    {
        if (Instance == null) // �÷��̾��� �ν��Ͻ��� ���� �������� �ʾ��� ��
        {
            Instance = this; // ���� ��ũ��Ʈ�� �ν��Ͻ��� �Ҵ��Ͽ� �̱��� ������ ����
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �÷��̾� ���� ������Ʈ�� �ı����� �ʵ��� ����
        }

    }
    void Start()
    {
        
        PlayBGM("BGM_Lobby");
    }
    void Update()
    {
        
        Bgm.volume = GameManager.Instance.Sound_BGM;

    }

    public void EffectLatePlay(string name, float time)
    {
        StartCoroutine(LateEffect(name, time));
    }

    IEnumerator LateEffect(string name, float time)
    {
        yield return new WaitForSeconds(time);
        PlayEffect(name);
    }

    public void PlayEffect(string name)
    {
        // EffectClip �迭���� name�� ���� �̸��� ���� AudioClip�� ã���ϴ�.
        AudioClip clipToPlay = null;
        foreach (var clip in EffectClip)
        {
            if (clip.name == name)
            {
                clipToPlay = clip;
                break;
            }
        }

        if (clipToPlay != null)
        {
            // ã�� AudioClip�� Effect AudioSource���� ����մϴ�.
            GameObject sound = Instantiate(Effect, EffectPos.transform);
            sound.GetComponent<AudioSource>().clip = clipToPlay;
            sound.GetComponent<AudioSource>().volume = GameManager.Instance.Sound_Effect;
            sound.GetComponent<AudioSource>().Play();
            Destroy(sound, 14f);
        }
        else
        {
            Debug.LogWarning("Could not find AudioClip with name: " + name);
        }
    }

    public void PlayBGM(string name)
    {
        if (BGMClip.Length > 0)
        {
            AudioClip clipToPlay = null;
            foreach (var clip in BGMClip)
            {
                if (clip.name == name)
                {
                    clipToPlay = clip;
                    break;
                }
            }
            Bgm.clip = clipToPlay;
            Bgm.Play();
        }
        else
        {
            Debug.LogWarning("BGMClip array is empty. Cannot play random BGM.");
        }
    }
}