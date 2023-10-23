using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Player_Button : MonoBehaviour
{
    private Button button;

    LobbyManager lobby;

    
    [SerializeField] Transform PlayerSpawnPos;
    [SerializeField] AnimatorController controller;
    // [SerializeField] AnimatorOverrideController LobbyController;

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
            case "Back":
                lobby.SetOff(lobby.Player);
                lobby.SetOn(lobby.First);
                break;

            case "Archer":
                Debug.Log("���Ĵ� �𵨸� �����Դϴ�");
                break;

            case "Assasine":
                SpawnCharactor(gameObject.name);
                break;

            case "barbarian":
                SpawnCharactor(gameObject.name);
                break;

            case "Cleric":
                SpawnCharactor(gameObject.name);
                break;

            case "Mage":
                SpawnCharactor(gameObject.name);
                break;

            case "Necromancer":
                SpawnCharactor(gameObject.name);
                break;

            case "paladin":
                SpawnCharactor(gameObject.name);
                break;

            case "Warrior":
                SpawnCharactor(gameObject.name);
                break;

        }
        //SpawnCharactor(gameObject.name);
    }
   
    
    private void SpawnCharactor(string Name)
    {
        if(LobbyManager.Instance.PlayerCharactor != null)
        { 
            Destroy(LobbyManager.Instance.PlayerCharactor);
        }
        foreach (GameObject job in GameManager.Instance.PlayerCharactor)
        {
            if (job.name == Name)
            {
                GameObject Player = Instantiate(job, PlayerSpawnPos.position, Quaternion.Euler(0,90,0));
                Player.GetComponent<Animator>().runtimeAnimatorController = controller;
                LobbyManager.Instance.PlayerCharactor = Player;
                GameManager.Instance.CurrentPlayerCharactor= job;
            }
        }
        DataManager.Instance.InputPlayerInfo(GameManager.Instance.CurrentPlayerCharactor.name);
    }
}
