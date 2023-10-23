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
        if (Instance == null) // 플레이어의 인스턴스가 아직 생성되지 않았을 때
        {
            Instance = this; // 현재 스크립트의 인스턴스를 할당하여 싱글톤 패턴을 구현
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 플레이어 게임 오브젝트가 파괴되지 않도록 설정
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
        // EffectClip 배열에서 name과 같은 이름을 가진 AudioClip을 찾습니다.
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
            // 찾은 AudioClip을 Effect AudioSource에서 재생합니다.
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