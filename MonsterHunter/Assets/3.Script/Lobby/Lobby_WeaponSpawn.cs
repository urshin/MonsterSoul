using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_WeaponSpawn : MonoBehaviour
{
    GameManager GM; // GameManager �ν��Ͻ��� ���� ������ �����ϴ� ����
    LobbyManager Lobby; // LobbyManager �ν��Ͻ��� ���� ������ �����ϴ� ����

    [SerializeField] GameObject Weapon; // ���⸦ ��Ÿ���� �������� �Ҵ��ϴ� ����

    [SerializeField] RenderTexture RenderTexture; // ���� �ؽ�ó�� �Ҵ��ϴ� ����
    [SerializeField] GameObject WeaponShow; // ���� ���� �����ִ� ���� ������Ʈ �������� �Ҵ��ϴ� ����
    [SerializeField] Transform WeaponShowTransform; // ���� ���� ǥ���� ��ġ�� ��Ÿ���� Transform

    void Start()
    {
        // GameManager�� LobbyManager �ν��Ͻ��� �Ҵ�
        GM = GameManager.Instance;
        Lobby = LobbyManager.Instance;

        // ���⸦ �����ϴ� �Լ� ȣ��
        SpawnWeapon();
    }

    void Update()
    {
        // ������Ʈ �Լ�
        // (����� �ƹ� ���� ����)
    }

    void SpawnWeapon()
    {
        // GM.WeaponList�� �ִ� ��� ���⿡ ���� ����
        foreach (var WeaponInfo in GM.WeaponList)
        {
            // Weapon �������� �����Ͽ� Info ���� ������Ʈ�� �����ϰ�, �̸� ����
            GameObject Info = Instantiate(Weapon, transform);
            Info.name = WeaponInfo.name;

            // ���� �̸����� "Sword_"�� �����Ͽ� ������ �̸� ����
            string originalText = WeaponInfo.name;
            string modifiedText = originalText.Replace("Sword_", ""); // "Sword_"�� �� ���ڿ��� ��ü�Ͽ� ����


            // Info ���� ������Ʈ�� �ڽ� ������Ʈ�� Text ������Ʈ�� ���� ���� �̸��� ���ݷ��� ����
            Info.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = modifiedText;
            Info.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = WeaponInfo.GetComponent<Weapon>().Attack.ToString();
            // ������ ���� ��ġ ����
            Vector3 positionOffset = new Vector3(11 * GM.WeaponList.IndexOf(WeaponInfo), 0, 0);
            Debug.Log(GM.WeaponList.IndexOf(WeaponInfo));
            // ���� �ؽ�ó�� �����Ͽ� te ������ �Ҵ�
            RenderTexture te = Instantiate(RenderTexture, transform);
            // WeaponShow ���� ������Ʈ�� ModelingShow ��ũ��Ʈ�� ī�޶� ���� �ؽ�ó�� �Ҵ�
            WeaponShow.GetComponent<ModelingShow>().cam.GetComponent<Camera>().targetTexture = te;
            // ���� ���� �����ִ� ���� ������Ʈ�� �����ϰ� ��ġ ����
            GameObject Show = Instantiate(WeaponShow, WeaponShowTransform.position + positionOffset, Quaternion.identity);

            // ModelingShow ��ũ��Ʈ�� GetWeapon ������ ����
            Show.GetComponent<ModelingShow>().GetWeapon = WeaponInfo;



            // Info ���� ������Ʈ�� �ڽ� ������Ʈ�� RawImage�� �ؽ�ó�� ���� �ؽ�ó�� ����
            Info.transform.GetChild(1).GetComponent<RawImage>().texture = te;
        }
    }
}