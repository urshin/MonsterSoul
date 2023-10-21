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
        button = GetComponent<Button>(); //��ư component ��������
        lobby = LobbyManager.Instance; //�κ�޴��� ����
        button.onClick.AddListener(Click); //���ڰ� ���� �� �Լ� ȣ��

    }



    void Click() //Ŭ���ϴ� ��ư�� ���� �ٸ� ��� ����
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
