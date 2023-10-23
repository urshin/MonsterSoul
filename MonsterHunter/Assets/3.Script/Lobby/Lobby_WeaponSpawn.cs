using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_WeaponSpawn : MonoBehaviour
{
    GameManager GM; // GameManager 인스턴스에 대한 참조를 보유하는 변수
    LobbyManager Lobby; // LobbyManager 인스턴스에 대한 참조를 보유하는 변수

    [SerializeField] GameObject Weapon; // 무기를 나타내는 프리팹을 할당하는 변수

    [SerializeField] RenderTexture RenderTexture; // 렌더 텍스처를 할당하는 변수
    [SerializeField] GameObject WeaponShow; // 무기 모델을 보여주는 게임 오브젝트 프리팹을 할당하는 변수
    [SerializeField] Transform WeaponShowTransform; // 무기 모델을 표시할 위치를 나타내는 Transform

    void Start()
    {
        // GameManager와 LobbyManager 인스턴스를 할당
        GM = GameManager.Instance;
        Lobby = LobbyManager.Instance;

        // 무기를 생성하는 함수 호출
        SpawnWeapon();
    }

    void Update()
    {
        // 업데이트 함수
        // (현재는 아무 동작 없음)
    }

    void SpawnWeapon()
    {
        // GM.WeaponList에 있는 모든 무기에 대한 루프
        foreach (var WeaponInfo in GM.WeaponList)
        {
            // Weapon 프리팹을 복제하여 Info 게임 오브젝트를 생성하고, 이름 설정
            GameObject Info = Instantiate(Weapon, transform);
            Info.name = WeaponInfo.name;

            // 무기 이름에서 "Sword_"를 제거하여 수정된 이름 생성
            string originalText = WeaponInfo.name;
            string modifiedText = originalText.Replace("Sword_", ""); // "Sword_"를 빈 문자열로 대체하여 제거


            // Info 게임 오브젝트의 자식 오브젝트의 Text 컴포넌트에 원래 무기 이름과 공격력을 설정
            Info.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = modifiedText;
            Info.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = WeaponInfo.GetComponent<Weapon>().Attack.ToString();
            // 순번에 따라 위치 조정
            Vector3 positionOffset = new Vector3(11 * GM.WeaponList.IndexOf(WeaponInfo), 0, 0);
            Debug.Log(GM.WeaponList.IndexOf(WeaponInfo));
            // 렌더 텍스처를 복제하여 te 변수에 할당
            RenderTexture te = Instantiate(RenderTexture, transform);
            // WeaponShow 게임 오브젝트의 ModelingShow 스크립트의 카메라에 렌더 텍스처를 할당
            WeaponShow.GetComponent<ModelingShow>().cam.GetComponent<Camera>().targetTexture = te;
            // 무기 모델을 보여주는 게임 오브젝트를 생성하고 위치 설정
            GameObject Show = Instantiate(WeaponShow, WeaponShowTransform.position + positionOffset, Quaternion.identity);

            // ModelingShow 스크립트의 GetWeapon 변수를 설정
            Show.GetComponent<ModelingShow>().GetWeapon = WeaponInfo;



            // Info 게임 오브젝트의 자식 오브젝트의 RawImage의 텍스처를 렌더 텍스처로 설정
            Info.transform.GetChild(1).GetComponent<RawImage>().texture = te;
        }
    }
}