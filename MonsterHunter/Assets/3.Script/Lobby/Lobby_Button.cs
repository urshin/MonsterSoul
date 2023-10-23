using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        SoundManager.Instance.PlayEffect("BTN");

        switch (gameObject.name)
        {
            case "Start":
                ChangeScene();
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

    void ChangeScene()
    {
       if(GameManager.Instance.CurrentPlayerCharactor!= null&& GameManager.Instance.CurrentWeapon !=null && GameManager.Instance.CurrentBoss !=null)
        {
            string name = GameManager.Instance.CurrentBoss.name.Trim();
        SceneManager.LoadScene(name);

        }
       else
        {
            if(GameManager.Instance.CurrentPlayerCharactor == null)
            {
                Debug.Log("캐릭터 선택 필요");
            }
            if (GameManager.Instance.CurrentWeapon == null)
            {
                Debug.Log("무기 선택 필요");
            }
            if (GameManager.Instance.CurrentBoss == null)
            {
                Debug.Log("보스 선택 필요");
            }
        }
    }

}
