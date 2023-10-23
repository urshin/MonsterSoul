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
