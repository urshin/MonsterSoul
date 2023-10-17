using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public static Player Instance; // �÷��̾��� �ν��Ͻ��� �����ϴ� ���� ����

    public void Awake()
    {
        if (Instance == null) // �÷��̾��� �ν��Ͻ��� ���� �������� �ʾ��� ��
        {
            Instance = this; // ���� ��ũ��Ʈ�� �ν��Ͻ��� �Ҵ��Ͽ� �̱��� ������ ����
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �÷��̾� ���� ������Ʈ�� �ı����� �ʵ��� ����
        }
    }

    public float PlayerHP; // �÷��̾��� ü��
    public float PlayerSpeed; // �÷��̾��� �̵� �ӵ�
    public float PlayerAttack; // �÷��̾��� ���ݷ�
    public float PlayerMotionDamage; // �÷��̾��� ��� ������
    GameObject WeaponScript; // ���� ��ũ��Ʈ�� ������ ����

    public GameObject Weapon; // ���� ������
    public GameObject CurrentWeapon; // ���� ������ ���⸦ ������ ����

   // public float PlayerTotalDamage;

    [SerializeField] Transform WeaponPos_r; // ���⸦ ��ȯ�� ��ġ (������ �� ��ġ)

    private void Start()
    {
        SpawnWeapon(); // ���⸦ �����ϴ� �Լ� ȣ��
        WeaponScript = GameObject.FindGameObjectWithTag("Weapon"); // "Weapon" �±׸� ���� ������Ʈ�� ã�� WeaponScript ������ �Ҵ�
        CurrentWeapon = GameObject.FindGameObjectWithTag("Weapon"); // "Weapon" �±׸� ���� ������Ʈ�� ���� ����� ����
    }

    void SpawnWeapon()
    {
        Instantiate(Weapon, WeaponPos_r); // Weapon �������� WeaponPos_r ��ġ�� ����
    }

    void DestroyWeapon()
    {
        Destroy(CurrentWeapon); // ���� ���� ������Ʈ�� �ı�
    }

    public void TotalDamage()
    {
        // ������ ���ݷ�, �÷��̾��� ���ݷ�, �÷��̾��� ��� �������� �ջ��� ���� �α׷� ���
        Debug.Log(WeaponScript.GetComponent<Weapon>().Attack + PlayerAttack + PlayerMotionDamage);
        //PlayerTotalDamage = WeaponScript.GetComponent<Weapon>().Attack + PlayerAttack + PlayerMotionDamage;
    }

    public float PlayerTotalDamage()
    {
        return WeaponScript.GetComponent<Weapon>().Attack + PlayerAttack + PlayerMotionDamage; 
    }


}