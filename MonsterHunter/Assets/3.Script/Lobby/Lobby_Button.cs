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
            // 씬이 존재하는지 확인
            if (SceneExists(name))
            {
                SceneManager.LoadScene(name);
            }
            else
            {
                GameManager.Instance.SpawnMessage("해당하는 보스는 아직 구현이 안되어 있습니다. 신속히 추가하겠습니다");
            }
        }
       else
        {
            if(GameManager.Instance.CurrentPlayerCharactor == null)
            {
                GameManager.Instance.SpawnMessage("플레이어를 선택해 주세요");
            }
            if (GameManager.Instance.CurrentWeapon == null)
            {
                GameManager.Instance.SpawnMessage("무기를 선택해 주세요");
            }
            if (GameManager.Instance.CurrentBoss == null)
            {
                GameManager.Instance.SpawnMessage("보스를 선택해주세요");
            }
        }
    }
    bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) //씬의 카운트 만큼
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); 
            string sceneNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(scenePath);//i인덱스의 씬의 이름 가져오기 

            if (sceneNameWithoutExtension == sceneName) //씬의 이름과 인자값이 같은 경우 true 반환
            {
                return true;
            }
        }
        return false;
    }
}
