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
                lobby.SetOff(lobby.Player);
                lobby.SetOn(lobby.First);
                break;

            case "Archer":
                Debug.Log("아쳐는 모델링 오류입니다");
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
