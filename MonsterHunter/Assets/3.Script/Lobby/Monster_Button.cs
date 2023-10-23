using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Button : MonoBehaviour
{
    private Button button;

    LobbyManager lobby;


    
    // [SerializeField] AnimatorOverrideController LobbyController;

    void Start()
    {
        button = GetComponent<Button>(); //버튼 component 가져오기
        lobby = LobbyManager.Instance; //로비메니저 연결
        button.onClick.AddListener(Click); //인자가 없을 때 함수 호출

    }



    void Click() //클릭하는 버튼에 따라 다른 기능 구현
    {
        SoundManager.Instance.PlayEffect("BTN");

        switch (gameObject.name)
        {
            case "Back":
                lobby.SetOff(lobby.Monster);
                lobby.SetOn(lobby.First);
                break;



        }
        
        foreach (GameObject Boss in GameManager.Instance.BossList)
        {
            if (Boss.name == gameObject.name)
            {
                GameManager.Instance.CurrentBoss = Boss;
            }
        }
    }


   
}
