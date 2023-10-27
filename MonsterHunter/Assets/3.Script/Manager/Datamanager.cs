using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public void Awake()
    {
        if (Instance == null) // 플레이어의 인스턴스가 아직 생성되지 않았을 때
        {
            Instance = this; // 현재 스크립트의 인스턴스를 할당하여 싱글톤 패턴을 구현
        }
    }

    GameManager GM;
    string PlayerInformation;

    void Start()
    {
        // 파일 경로 설정
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
        // 아마 게임오브젝트로 읽은 name에 clone이나 다른 이름이 들어가 있어서 그런든 contain으로 해보기
        // 리소스 폴더 내에서 텍스트 파일 읽기
        TextAsset textAsset = Resources.Load<TextAsset>(PlayerInformation);

        if (textAsset != null)
        {
            // 파일 내용을 줄 단위로 분리
            string[] lines = textAsset.text.Split('\n');

            bool foundCharacter = false; // Name과 일치하는 캐릭터 정보를 찾았는지 여부

            foreach (string line in lines)
            {
                Debug.Log(0);
                // 앞 5글자 추출 후 대소문자 구분 없이 비교
                string compareName = Name.Substring(0, 5).ToLower(); // Name을 소문자로 변환
                if (line.ToLower().Contains(compareName))
                {
                    foundCharacter = true;
                }
                // 해당 캐릭터 정보를 분리
                string[] info = line.Split(':');
                Debug.Log(info.Length);
                Debug.Log(info[0]);
                if (info.Length >= 2)
                {
                    string attribute = info[0].Trim();
                    float value = float.Parse(info[1].Trim());
                    Debug.Log(2);
                    // 해당 캐릭터 변수에 값을 대입
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
                    // Name과 일치하는 캐릭터 정보를 찾았고, 빈 줄을 만난 경우 종료
                    break;
                }
            }

            // 리소스 해제
            Resources.UnloadAsset(textAsset);
        }
        else
        {
            Debug.LogError("File not found: " + PlayerInformation);
        }
    }
}