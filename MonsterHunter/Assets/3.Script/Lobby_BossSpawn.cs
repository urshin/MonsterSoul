using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_BossSpawn : MonoBehaviour
{
    GameManager GM; // GameManager 인스턴스에 대한 참조를 보유하는 변수
    LobbyManager Lobby; // LobbyManager 인스턴스에 대한 참조를 보유하는 변수

    [SerializeField] GameObject BossTemplet;
   

    void Start()
    {
        // GameManager와 LobbyManager 인스턴스를 할당
        GM = GameManager.Instance;
        Lobby = LobbyManager.Instance;
        SpawnBossInfo();



    }

    void Update()
    {
        // 업데이트 함수
        // (현재는 아무 동작 없음)
    }

    void SpawnBossInfo()
    {

        foreach(var Boss in GM.BossList)
        {
            GameObject BossInfo = Instantiate(BossTemplet, transform);
            BossInfo.name = Boss.name;
            BossInfo.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text = Boss.name;
            //나중에 보스 설명 란도 연결해주기




        }

    }

  
}
