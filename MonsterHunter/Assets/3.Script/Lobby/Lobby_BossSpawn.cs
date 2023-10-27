using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LobbyBossSpawn : MonoBehaviour
{
    private GameManager GM;     // GameManager �ν��Ͻ��� ���� ������ �����ϴ� ����
    private LobbyManager Lobby; // LobbyManager �ν��Ͻ��� ���� ������ �����ϴ� ����

    [SerializeField] private GameObject bossTemplate;
    private string bossInformation = "Monster/MonsterInfo";

    private void Start()
    {
        // GameManager�� LobbyManager �ν��Ͻ��� �Ҵ�
        GM = GameManager.Instance;
        Lobby = LobbyManager.Instance;

        SpawnBossInfo();
    }

    private void Update()
    {
        // ������Ʈ �Լ�
        // (����� �ƹ� ���� ����)
    }

    private void SpawnBossInfo()
    {
        TextAsset bossInfoTextAsset = Resources.Load<TextAsset>(bossInformation); // �ؽ�Ʈ ���Ͽ��� ���� ��������
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
                        continue; // ���� �ٺ��� ������ ����
                    }

                    if (foundBoss && string.IsNullOrWhiteSpace(line))
                    {
                        break; // ����ִ� ���� ������ ���� ��
                    }

                    if (foundBoss)
                    {
                        bossDescription += line + "\n"; // ���� �� �߰�
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