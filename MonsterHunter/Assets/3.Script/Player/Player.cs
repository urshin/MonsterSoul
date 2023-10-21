using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public static Player Instance; // 플레이어의 인스턴스를 저장하는 정적 변수

    public void Awake()
    {
        if (Instance == null) // 플레이어의 인스턴스가 아직 생성되지 않았을 때
        {
            Instance = this; // 현재 스크립트의 인스턴스를 할당하여 싱글톤 패턴을 구현
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 플레이어 게임 오브젝트가 파괴되지 않도록 설정
        }
    }

    public float PlayerHP; // 플레이어의 체력
    public float PlayerSpeed; // 플레이어의 이동 속도
    public float PlayerAttack; // 플레이어의 공격력
    public float PlayerMotionDamage; // 플레이어의 모션 데미지
    GameObject WeaponScript; // 무기 스크립트를 저장할 변수

    public GameObject Weapon; // 무기 프리팹
    public GameObject CurrentWeapon; // 현재 장착된 무기를 저장할 변수

    public bool isPlayerBulling;

    [SerializeField] Transform WeaponPos_r; // 무기를 소환할 위치 (오른쪽 손 위치)

    public Animator anime;
    private void Start()
    {
        SpawnWeapon(); // 무기를 생성하는 함수 호출
        WeaponScript = GameObject.FindGameObjectWithTag("Weapon"); // "Weapon" 태그를 가진 오브젝트를 찾아 WeaponScript 변수에 할당
        CurrentWeapon = GameObject.FindGameObjectWithTag("Weapon"); // "Weapon" 태그를 가진 오브젝트를 현재 무기로 설정\
        isPlayerBulling = false;//공격 당하지 않음
        anime = GetComponentInChildren<Animator>();
    }

    void SpawnWeapon()
    {
        Instantiate(Weapon, WeaponPos_r); // Weapon 프리팹을 WeaponPos_r 위치에 생성
    }

    void DestroyWeapon()
    {
        Destroy(CurrentWeapon); // 현재 무기 오브젝트를 파괴
    }

    public void TotalDamage()
    {
        // 무기의 공격력, 플레이어의 공격력, 플레이어의 모션 데미지를 합산한 값을 로그로 출력
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
                Debug.Log("공습경보");
                PlayerHP -= SandWormBoss.Instance.SandWormAttackDamage;
                

            }
        }
    }

    public void StartPattern(string name)
    {
       // Debug.Log("패턴 시작" + name);
        anime.SetBool(name, true);
    }
    public void StopPattern(string name)
    {
       // Debug.Log("패턴 끝" + name);

        anime.SetBool(name, false);
    }


    //private void OnCollisionEnter(Collision collision)
    //{

    //    if (SandWormBoss.Instance.IsAttacking && !isPlayerBulling)
    //    {
    //        if (collision.gameObject.CompareTag("Enemy"))
    //        {
    //            isPlayerBulling = true;
    //            Debug.Log("공습경보");
    //            PlayerHP -= SandWormBoss.Instance.SandWormAttackDamage;

    //        }
    //    }
    //}

}