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
    GameManager GM;

    public float PlayerHP; // �÷��̾��� ü��
    public float PlayerSpeed; // �÷��̾��� �̵� �ӵ�
    public float PlayerAttack; // �÷��̾��� ���ݷ�
    public float PlayerrStamina; // �÷��̾��� ���׹̳�
    public float PlayerMotionDamage; // �÷��̾��� ��� ������
    GameObject WeaponScript; // ���� ��ũ��Ʈ�� ������ ����

    public GameObject Weapon; // ���� ������
    public GameObject CurrentWeapon; // ���� ������ ���⸦ ������ ����

    public bool isPlayerBulling;

    [SerializeField] GameObject WeaponPos_r; // ���⸦ ��ȯ�� ��ġ (������ �� ��ġ)
    public GameObject PlayerAvatar; //�ƹ�Ÿ

    public Animator anime;
    private void Start()
    {
        GM = GameManager.Instance;
        InitializedPlayerInfo();
        Invoke("LateStart", 0.1f);
        isPlayerBulling = false;//���� ������ ����
    }


    void LateStart()
    {
        SpawnPlayerAvatar();
        SpawnWeapon(); // ���⸦ �����ϴ� �Լ� ȣ��
        WeaponScript = GameObject.FindGameObjectWithTag("Weapon"); // "Weapon" �±׸� ���� ������Ʈ�� ã�� WeaponScript ������ �Ҵ�
        CurrentWeapon = GameObject.FindGameObjectWithTag("Weapon"); // "Weapon" �±׸� ���� ������Ʈ�� ���� ����� ����\
    }
    void InitializedPlayerInfo()
    {
        PlayerHP = GM.PlayerHp;
        PlayerSpeed = GM.PlayerSpeed;
        PlayerAttack = GM.PlayerAttack;
        PlayerrStamina = GM.PlayerStamina;
    }

    void SpawnPlayerAvatar()
    {
        PlayerAvatar = Instantiate(GameManager.Instance.CurrentPlayerCharactor, gameObject.transform);
        // �θ�-�ڽ� ���� ����
        PlayerAvatar.transform.parent = gameObject.transform;
        PlayerAvatar.GetComponent<Animator>().runtimeAnimatorController = GameManager.Instance.Worrior;
        anime = PlayerAvatar.GetComponent<Animator>();
    }
    void SpawnWeapon()
    {
        WeaponPos_r = GameObject.FindGameObjectWithTag("WeaponPos_R");
        Instantiate(GameManager.Instance.CurrentWeapon, WeaponPos_r.transform); // Weapon �������� WeaponPos_r ��ġ�� ����
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