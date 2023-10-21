using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_Button : MonoBehaviour
{
    private Button button;

    LobbyManager lobby;
    void Start()
    {
        button = GetComponent<Button>(); //버튼 component 가져오기
        lobby = LobbyManager.Instance; //로비메니저 연결
        button.onClick.AddListener(Click); //인자가 없을 때 함수 호출
    }

    void Click() //클릭하는 버튼에 따라 다른 기능 구현
    {
        switch (gameObject.name)
        {
            case "Start":
                //체인지 씬
                break;
            case "Player":
                lobby.SetOff(lobby.First);
                lobby.SetOn(lobby.Player);
                break;
            case "Monster":
                lobby.SetOff(lobby.First);
                lobby.SetOn(lobby.Monster);
                break;
            case "Weapon":
                lobby.SetOff(lobby.First);
                lobby.SetOn(lobby.Weapon);
                break;
            case "Setting":
                lobby.SetOff(lobby.First);
                lobby.SetOn(lobby.Setting);
                break;
            case "EndGame":
                lobby.SetOff(lobby.First);

                break;

        }
    }
}
