using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public bool isPlayerBulling;

    [SerializeField] Transform WeaponPos_r; // ���⸦ ��ȯ�� ��ġ (������ �� ��ġ)

    public Animator anime;
    private void Start()
    {
        SpawnWeapon(); // ���⸦ �����ϴ� �Լ� ȣ��
        WeaponScript = GameObject.FindGameObjectWithTag("Weapon"); // "Weapon" �±׸� ���� ������Ʈ�� ã�� WeaponScript ������ �Ҵ�
        CurrentWeapon = GameObject.FindGameObjectWithTag("Weapon"); // "Weapon" �±׸� ���� ������Ʈ�� ���� ����� ����\
        isPlayerBulling = false;//���� ������ ����
        anime = GetComponentInChildren<Animator>();
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
        //Debug.Log(WeaponScript.GetComponent<Weapon>().Attack + PlayerAttack + PlayerMotionDamage);
        //PlayerTotalDamage = WeaponScript.GetComponent<Weapon>().Attack + PlayerAttack + PlayerMotionDamage;
    }

    public float PlayerTotalDamage()
    {
        return WeaponScript.GetComponent<Weapon>().Attack + PlayerAttack + PlayerMotionDamage;
    }

    public bool IsDown =false;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (SandWormBoss.Instance.IsAttacking && !isPlayerBulling)
        {
            if (other.gameObject.CompareTag("Enemy")|| other.gameObject.CompareTag("EnemyAttack"))
            {
                isPlayerBulling = true;
                IsDown=true;
                Debug.Log("�����溸");
                PlayerHP -= SandWormBoss.Instance.SandWormAttackDamage;
                

            }
        }
    }

    public void StartPattern(string name)
    {
       // Debug.Log("���� ����" + name);
        anime.SetBool(name, true);
    }
    public void StopPattern(string name)
    {
       // Debug.Log("���� ��" + name);

        anime.SetBool(name, false);
    }


    //private void OnCollisionEnter(Collision collision)
    //{

    //    if (SandWormBoss.Instance.IsAttacking && !isPlayerBulling)
    //    {
    //        if (collision.gameObject.CompareTag("Enemy"))
    //        {
    //            isPlayerBulling = true;
    //            Debug.Log("�����溸");
    //            PlayerHP -= SandWormBoss.Instance.SandWormAttackDamage;

    //        }
    //    }
    //}

}