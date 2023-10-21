using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_Button : MonoBehaviour
{
    private Button button;

    LobbyManager lobby;


    [SerializeField] Transform WeaponPos;
    // [SerializeField] AnimatorOverrideController LobbyController;

    void Start()
    {
        button = GetComponent<Button>(); //버튼 component 가져오기
        lobby = LobbyManager.Instance; //로비메니저 연결
        button.onClick.AddListener(Click); //인자가 없을 때 함수 호출

    }



    void Click() //클릭하는 버튼에 따라 다른 기능 구현
    {

        switch (gameObject.name)
        {
            case "Back":
                lobby.SetOff(lobby.Weapon);
                lobby.SetOn(lobby.First);
                break;

           

        }
        if (lobby.PlayerWeapon != null && gameObject.name != "Back")
        {
            Destroy(lobby.PlayerWeapon);
        }
        foreach (GameObject WeaponName in GameManager.Instance.WeaponList)
        {
            if (WeaponName.name == gameObject.name)
            {
                GameObject Weapon = Instantiate(WeaponName,lobby.PlayerWeaponSpawnPos.transform);
                lobby.PlayerWeapon = Weapon;
                Weapon.GetComponent<Rigidbody>().useGravity = false;
                Weapon.GetComponent<CapsuleCollider>().isTrigger= true;
                GameManager.Instance.CurrentWeapon = lobby.PlayerWeapon;
            }
        }
    }


    private void SpawnCharactor(string Name)
    {
        if (lobby.PlayerWeapon != null)
        {
            Destroy(lobby.PlayerWeapon);
        }
        foreach (GameObject WeaponName in GameManager.Instance.WeaponList)
        {
            if (WeaponName.name == Name)
            {
                GameObject Weapon = Instantiate(WeaponName, WeaponPos.position, Quaternion.Euler(0, 90, 0));
                lobby.PlayerWeapon = Weapon;
                GameManager.Instance.CurrentWeapon = lobby.PlayerWeapon;
            }
        }

    }
}
