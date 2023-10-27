using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public void Awake()
    {
        if (Instance == null) // �÷��̾��� �ν��Ͻ��� ���� �������� �ʾ��� ��
        {
            Instance = this; // ���� ��ũ��Ʈ�� �ν��Ͻ��� �Ҵ��Ͽ� �̱��� ������ ����
        }
    }

    GameManager GM;
    string PlayerInformation;

    void Start()
    {
        // ���� ��� ����
        PlayerInformation = "PlayerCharactorData/PlayerCharactorInfo";

        GM = GameManager.Instance;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    InputPlayerInfo(GameManager.Instance.CurrentPlayerCharactor.name);
        //}
    }

    public void InputPlayerInfo(string Name)
    {
        // �Ƹ� ���ӿ�����Ʈ�� ���� name�� clone�̳� �ٸ� �̸��� �� �־ �׷��� contain���� �غ���
        // ���ҽ� ���� ������ �ؽ�Ʈ ���� �б�
        TextAsset textAsset = Resources.Load<TextAsset>(PlayerInformation);

        if (textAsset != null)
        {
            // ���� ������ �� ������ �и�
            string[] lines = textAsset.text.Split('\n');

            bool foundCharacter = false; // Name�� ��ġ�ϴ� ĳ���� ������ ã�Ҵ��� ����

            foreach (string line in lines)
            {
                Debug.Log(0);
                // �� 5���� ���� �� ��ҹ��� ���� ���� ��
                string compareName = Name.Substring(0, 5).ToLower(); // Name�� �ҹ��ڷ� ��ȯ
                if (line.ToLower().Contains(compareName))
                {
                    foundCharacter = true;
                }
                // �ش� ĳ���� ������ �и�
                string[] info = line.Split(':');
                Debug.Log(info.Length);
                Debug.Log(info[0]);
                if (info.Length >= 2)
                {
                    string attribute = info[0].Trim();
                    float value = float.Parse(info[1].Trim());
                    Debug.Log(2);
                    // �ش� ĳ���� ������ ���� ����
                    switch (attribute)
                    {
                        case "PlayerHp":
                            GM.PlayerHp = value;
                            break;
                        case "PlayerSpeed":
                            GM.PlayerSpeed = value;
                            break;
                        case "PlayerAttack":
                            GM.PlayerAttack = value;
                            break;
                        case "PlayerStamina":
                            GM.PlayerStamina = value;
                            break;
                        case "JumpPower":
                            GM.PlayerJumpPower = value;
                            break;
                        case "PlayerMaximumItem":
                            GM.PlayerMaximumItem = value;
                            break;
                        default:
                            Debug.LogWarning("Unknown attribute: " + attribute);
                            break;
                    }
                }
                else if (foundCharacter && string.IsNullOrEmpty(line.Trim()))
                {
                    // Name�� ��ġ�ϴ� ĳ���� ������ ã�Ұ�, �� ���� ���� ��� ����
                    break;
                }
            }

            // ���ҽ� ����
            Resources.UnloadAsset(textAsset);
        }
        else
        {
            Debug.LogError("File not found: " + PlayerInformation);
        }
    }
}