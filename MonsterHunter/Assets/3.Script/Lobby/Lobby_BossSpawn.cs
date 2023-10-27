using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LobbyBossSpawn : MonoBehaviour
{
    private GameManager GM;     // GameManager 인스턴스에 대한 참조를 보유하는 변수
    private LobbyManager Lobby; // LobbyManager 인스턴스에 대한 참조를 보유하는 변수

    [SerializeField] private GameObject bossTemplate;
    private string bossInformation = "Monster/MonsterInfo";

    private void Start()
    {
        // GameManager와 LobbyManager 인스턴스를 할당
        GM = GameManager.Instance;
        Lobby = LobbyManager.Instance;

        SpawnBossInfo();
    }

    private void Update()
    {
        // 업데이트 함수
        // (현재는 아무 동작 없음)
    }

    private void SpawnBossInfo()
    {
        TextAsset bossInfoTextAsset = Resources.Load<TextAsset>(bossInformation); // 텍스트 파일에서 정보 가져오기
        Sprite[] bossInfoImages = Resources.LoadAll<Sprite>("Monster");
        Debug.Log(bossInfoImages.Length);

        if (bossInfoTextAsset != null)
        {
            string bossInfoText = bossInfoTextAsset.text;
            string[] bossInfoLines = bossInfoText.Split('\n');

            foreach (var boss in GM.BossList)
            {
                string bossName = boss.name;
                string bossDescription = "";

                bool foundBoss = false;
                foreach (string line in bossInfoLines)
                {
                    if (line.Contains("Name:" + bossName))
                    {
                        foundBoss = true;
                        continue; // 다음 줄부터 설명을 읽음
                    }

                    if (foundBoss && string.IsNullOrWhiteSpace(line))
                    {
                        break; // 비어있는 줄이 나오면 설명 끝
                    }

                    if (foundBoss)
                    {
                        bossDescription += line + "\n"; // 설명에 줄 추가
                    }
                }

                GameObject bossInfo = Instantiate(bossTemplate, transform);
                bossInfo.name = boss.name;
                bossInfo.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = boss.name;
                bossInfo.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = bossDescription;

                foreach (Sprite image in bossInfoImages)
                {
                    if (image.name == boss.name)
                        bossInfo.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = image;
                }
            }
        }
    }
}