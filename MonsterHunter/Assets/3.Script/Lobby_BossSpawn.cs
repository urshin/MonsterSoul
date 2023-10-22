using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_BossSpawn : MonoBehaviour
{
    GameManager GM; // GameManager �ν��Ͻ��� ���� ������ �����ϴ� ����
    LobbyManager Lobby; // LobbyManager �ν��Ͻ��� ���� ������ �����ϴ� ����

    [SerializeField] GameObject BossTemplet;
   

    void Start()
    {
        // GameManager�� LobbyManager �ν��Ͻ��� �Ҵ�
        GM = GameManager.Instance;
        Lobby = LobbyManager.Instance;
        SpawnBossInfo();



    }

    void Update()
    {
        // ������Ʈ �Լ�
        // (����� �ƹ� ���� ����)
    }

    void SpawnBossInfo()
    {

        foreach(var Boss in GM.BossList)
        {
            GameObject BossInfo = Instantiate(BossTemplet, transform);
            BossInfo.name = Boss.name;
            BossInfo.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text = Boss.name;
            //���߿� ���� ���� ���� �������ֱ�




        }

    }

  
}
