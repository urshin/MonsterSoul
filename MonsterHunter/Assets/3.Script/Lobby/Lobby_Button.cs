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
            // ���� �����ϴ��� Ȯ��
            if (SceneExists(name))
            {
                SceneManager.LoadScene(name);
            }
            else
            {
                GameManager.Instance.SpawnMessage("�ش��ϴ� ������ ���� ������ �ȵǾ� �ֽ��ϴ�. �ż��� �߰��ϰڽ��ϴ�");
            }
        }
       else
        {
            if(GameManager.Instance.CurrentPlayerCharactor == null)
            {
                GameManager.Instance.SpawnMessage("�÷��̾ ������ �ּ���");
            }
            if (GameManager.Instance.CurrentWeapon == null)
            {
                GameManager.Instance.SpawnMessage("���⸦ ������ �ּ���");
            }
            if (GameManager.Instance.CurrentBoss == null)
            {
                GameManager.Instance.SpawnMessage("������ �������ּ���");
            }
        }
    }
    bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) //���� ī��Ʈ ��ŭ
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); 
            string sceneNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(scenePath);//i�ε����� ���� �̸� �������� 

            if (sceneNameWithoutExtension == sceneName) //���� �̸��� ���ڰ��� ���� ��� true ��ȯ
            {
                return true;
            }
        }
        return false;
    }
}
