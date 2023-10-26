using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;//싱글톤 


    public void Awake()
    {
        if (Instance == null) //정적으로 자신을 체크함, null인진
        {
            Instance = this; //이후 자기 자신을 저장함.

        }
    }

    public enum LobbyState
    {
        Logo,
        First,
        Setting,
        Player,
        Weapon,
        Monster,
        Start,
        EndGame,
    }

    public GameObject[] Logo;
    public GameObject[] Panel;
    public GameObject[] State;
    public GameObject[] First;
    public GameObject[] Setting;
    public GameObject[] Player;
    public GameObject[] Weapon;
    public GameObject[] Monster;



    public LobbyState CurrentLobbystate;
    public GameObject PlayerCharactor;
    public GameObject PlayerWeapon;
    public GameObject PlayerWeaponSpawnPos;
    


    private void Start()
    {
        Time.timeScale = 1.0f;
        CurrentLobbystate = LobbyState.Logo;
        StartCoroutine(FadeIn(Logo,2f));
        GameManager.Instance.cursorLocked = true;
        SoundManager.Instance.PlayBGM("BGM_Lobby");
    }
    private void Update()
    {
       if(Input.anyKeyDown && CurrentLobbystate == LobbyState.Logo)
        {
            StartCoroutine(FadeOut(Logo, 2f));
            Invoke("StartButton",2f);
        }



    }
    void StartButton()
    {
        SetOn(State);
        SetOn(First);
        CurrentLobbystate= LobbyState.First;
    }


    Image image;

    public void SetOn(GameObject[] gameObjects)
    {
        foreach(GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(true);
        }
       
    }
    public void SetOff(GameObject[] gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }
       
    }


    // 이미지를 서서히 밝아지게 하는 코루틴
    public IEnumerator FadeIn(GameObject[] GameImages, float duration)
    {
        foreach (GameObject GameImage in GameImages)
        {
            Image image = GameImage.GetComponent<Image>();
            image.gameObject.SetActive(true); // 이미지를 활성화합니다.

            float startAlpha = image.color.a;
            float targetAlpha = 1.0f;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
                Color newColor = image.color;
                newColor.a = newAlpha;
                image.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Color finalColor = image.color;
            finalColor.a = targetAlpha;
            image.color = finalColor;
        }
    }

    // 이미지를 서서히 어두워지게 하는 코루틴
    public IEnumerator FadeOut(GameObject[] GameImages, float duration)
    {
        foreach (GameObject GameImage in GameImages)
        {
            Image image = GameImage.GetComponent<Image>();
            float startAlpha = image.color.a;
            float targetAlpha = 0.0f;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
                Color newColor = image.color;
                newColor.a = newAlpha;
                image.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Color finalColor = image.color;
            finalColor.a = targetAlpha;
            image.color = finalColor;

            image.gameObject.SetActive(false); // 이미지를 비활성화합니다.
        }
    }





}
