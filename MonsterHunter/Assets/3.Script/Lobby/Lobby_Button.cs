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
        button = GetComponent<Button>(); //��ư component ��������
        lobby = LobbyManager.Instance; //�κ�޴��� ����
        button.onClick.AddListener(Click); //���ڰ� ���� �� �Լ� ȣ��
    }

    void Click() //Ŭ���ϴ� ��ư�� ���� �ٸ� ��� ����
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
                Debug.Log("ĳ���� ���� �ʿ�");
            }
            if (GameManager.Instance.CurrentWeapon == null)
            {
                Debug.Log("���� ���� �ʿ�");
            }
            if (GameManager.Instance.CurrentBoss == null)
            {
                Debug.Log("���� ���� �ʿ�");
            }
        }
    }

}
