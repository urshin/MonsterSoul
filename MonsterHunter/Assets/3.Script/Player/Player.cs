using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

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
    GameManager GM;

    public float PlayerHP; // 플레이어의 체력
    public float PlayerSpeed; // 플레이어의 이동 속도
    public float PlayerAttack; // 플레이어의 공격력
    public float PlayerrStamina; // 플레이어의 스테미너
    public float PlayerMotionDamage; // 플레이어의 모션 데미지
    public float PlayerMaximumItem;
    public float CurrentItemCount;
    GameObject WeaponScript; // 무기 스크립트를 저장할 변수

    [SerializeField] GameObject PlayerHP_Bar;
    [SerializeField] GameObject PlayerrStamina_Bar;

    // public GameObject Weapon; // 무기 프리팹
    public GameObject CurrentWeapon; // 현재 장착된 무기를 저장할 변수

    public bool isPlayerBulling;

    [SerializeField] GameObject WeaponPos_r; // 무기를 소환할 위치 (오른쪽 손 위치)
    public GameObject PlayerAvatar; //아바타

    public Animator anime;




    private void Start()
    {
        GM = GameManager.Instance;
        GM.PlayerLoad = false;
        InitializedPlayerInfo();
        Invoke("LateStart", 0.1f);
        isPlayerBulling = false;//공격 당하지 않음
    }


    void LateStart()
    {
        SpawnPlayerAvatar();
        SpawnWeapon(); // 무기를 생성하는 함수 호출
        WeaponScript = GameObject.FindGameObjectWithTag("Weapon"); // "Weapon" 태그를 가진 오브젝트를 찾아 WeaponScript 변수에 할당
        CurrentWeapon = GameObject.FindGameObjectWithTag("Weapon"); // "Weapon" 태그를 가진 오브젝트를 현재 무기로 설정\
        GM.PlayerLoad = true;

    }

    private void Update()
    {
        if (!GM.IsLoading)
        {

            PlayerHP_Bar.GetComponent<Slider>().value = PlayerHP;
            PlayerrStamina_Bar.GetComponent<Slider>().value = PlayerrStamina;






        }

    }

    void InitializedPlayerInfo()
    {
        PlayerHP = GM.PlayerHp;
        PlayerSpeed = GM.PlayerSpeed;
        PlayerAttack = GM.PlayerAttack;
        PlayerrStamina = GM.PlayerStamina;
        PlayerHP_Bar.GetComponent<Slider>().maxValue = PlayerHP;
        PlayerrStamina_Bar.GetComponent<Slider>().maxValue = PlayerrStamina;
        PlayerMaximumItem = GM.PlayerMaximumItem;
        CurrentItemCount = 0f;
    }

    void SpawnPlayerAvatar()
    {
        PlayerAvatar = Instantiate(GM.CurrentPlayerCharactor, gameObject.transform);
        // 부모-자식 관계 설정
        PlayerAvatar.transform.parent = gameObject.transform;
        PlayerAvatar.GetComponent<Animator>().runtimeAnimatorController = GM.Worrior;
        anime = PlayerAvatar.GetComponent<Animator>();
    }
    void SpawnWeapon()
    {
        WeaponPos_r = GameObject.FindGameObjectWithTag("WeaponPos_R");
        Instantiate(GM.CurrentWeapon, WeaponPos_r.transform); // Weapon 프리팹을 WeaponPos_r 위치에 생성
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

    public bool IsDown = false;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (SandWormBoss.Instance.IsAttacking && !isPlayerBulling)
        {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyAttack"))
            {
                isPlayerBulling = true;
                IsDown = true;
                Debug.Log("공습경보");
                PlayerHP -= SandWormBoss.Instance.SandWormAttackDamage;
                SoundManager.Instance.PlayEffect("GotDamage");

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

    public List<GameObject> ItemList = new List<GameObject>();

    [SerializeField] float Radius;//상점 근처 범위
    [SerializeField] LayerMask targetLayers; // 타겟 레이어 마스크 나중에 플레이어로 지정
    [SerializeField] GameObject StoreCanvas;
    [SerializeField] GameObject SellingThing;
    [SerializeField] bool StoreNear;

    private void StoreShow()
    {
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, Radius, targetLayers);

        foreach (Collider thing in nearbyTargets)
        {
            if (thing.CompareTag("Store"))
            {
                StoreNear = true;
                StoreCanvas = thing.GetComponent<Store>().Canvas;
                SellingThing = thing.GetComponent<Store>().SellingTing;
                return;



            }

        }
        StoreNear = false;

    }



    private void FixedUpdate()
    {
        BuyStore();

        if(Input.GetKeyDown (KeyCode.P))
        {
            ItemInvenUI.Clear();
            CurrentItemCount = 0;
        }

    }
    public List<GameObject> ItemInven = new List<GameObject>(); // 아이템 목록
    public List<GameObject> ItemInvenUI = new List<GameObject>(); // 아이템 UI 목록
    [SerializeField] RenderTexture RenderText; // 렌더 텍스처
    [SerializeField] GameObject ItemUI; // 아이템 UI 프리팹
    [SerializeField] GameObject ItemUISpawnPos; // 아이템 UI를 생성할 위치
    [SerializeField] RenderTexture RT; // 렌더 텍스처
    [SerializeField] GameObject Inventory; // 인벤토리 오브젝트
    [SerializeField] GameObject rawimage; // RawImage 오브젝트

    void SpawnItemUI(GameObject fruit)
    {
        // 아이템 UI를 생성하고 아이템 목록에 추가
        GameObject itemUIShow = Instantiate(ItemUI, ItemUISpawnPos.transform.position + new Vector3(3 * ItemInven.Count, 0, 0), Quaternion.identity);
        Instantiate(fruit, itemUIShow.transform);

        // 렌더 텍스처를 생성하고 카메라에 할당
        RT = Instantiate(RenderText);
        itemUIShow.GetComponentInChildren<Camera>().targetTexture = RT;

        // RawImage를 생성하고 렌더 텍스처를 할당한 후 UI 목록에 추가
        GameObject RI = Instantiate(rawimage, Inventory.transform);
        RI.GetComponent<RawImage>().texture = RT;

        ItemInvenUI.Add(RI);
        ItemInven.Add(itemUIShow);
        initializedItemUI();
    }

    void initializedItemUI()
    {
        float spacing = 180f; // 각 아이템 사이의 간격
        for (int i = 0; i < ItemInvenUI.Count; i++)
        {
            // 아이템 UI의 위치를 조정
            Vector3 newPosition = new Vector3(i * spacing, 0, 0);
            ItemInvenUI[i].transform.localPosition = newPosition;

          
            
        }
    }
    private void BuyStore()
    {
        StoreShow();
        if (StoreNear)
        {
            StoreCanvas.SetActive(true);

            // E 키를 눌렀을 때 아이템을 구매하고 UI에 추가
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (CurrentItemCount < PlayerMaximumItem)
                {
                    ItemList.Add(SellingThing);
                    CurrentItemCount++;
                    SpawnItemUI(SellingThing);
                }
            }
        }
        if (!StoreNear)
        {
            // 상점과 거리가 멀 경우 상점 UI 비활성화
            if (StoreCanvas != null)
            {
                StoreCanvas.SetActive(false);
            }
        }
    }


}