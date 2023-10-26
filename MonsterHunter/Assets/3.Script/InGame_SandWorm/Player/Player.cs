using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

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
    public float playerOriginHP; // 플레이어의 원래체력
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


    float LeftX;
    float RightX;
    bool IsPlayerDead;

    [Header("PlayerEffect")]
    [SerializeField] GameObject[] EffectBox;

    private void Start()
    {
        IsPlayerDead = false;
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


            BuyStore();
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (wheelInput > 0)
            {
                Debug.Log("휠 내림");
                LeftMove();
            }
            else if (wheelInput < 0)
            {
                // 휠을 당겨 올렸을 때의 처리 ↓
                RightMove();
            }
           
            if(PlayerHP <= 0)
            {
                if(!IsPlayerDead)
                {
                    InGameManager.Instance.SlowDowntime(InGameManager .Instance .PlayerEnding);
                    IsPlayerDead = true;    
                }
            }

        }

    }

    void InitializedPlayerInfo()
    {
        PlayerHP = GM.PlayerHp;
        playerOriginHP = GM.PlayerHp;
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

    void ItemEffect(int Itemnum)
    {
        Debug.Log(ItemInven[Itemnum].transform.GetChild(1).gameObject.name);
        string itemname = ItemInven[Itemnum].transform.GetChild(1).name.Replace("(Clone)", "").Trim();
        switch (itemname)
        {
            case "Banana":
                Debug.Log(itemname + "효과 발동!!");
                EffectOn(PlayerEffect.Buff);
                break;
            case "Orange":
                Debug.Log(itemname + "효과 발동!!");
                EffectOn(PlayerEffect.Heal);
                break;
        }


    }
    public enum PlayerEffect
    {
        Buff = 0,
        Heal = 1,
    }
    public PlayerEffect PlayerVFX;
    void EffectOn(PlayerEffect effect)
    {
        if (effect == PlayerEffect.Buff)
        {
            Instantiate(EffectBox[(int)PlayerEffect.Buff], transform);
            SoundManager.Instance.PlayEffect("Buff");
            PlayerAttack += 20;
            void OffBuff()
            {
                PlayerAttack -= 20;
            }
            Invoke("OffBuff",10);
        }

        if (effect == PlayerEffect.Heal)
        {
            Instantiate(EffectBox[(int)PlayerEffect.Heal], transform);
            SoundManager.Instance.PlayEffect("Heal");
            PlayerHP += playerOriginHP / 4; //플레이어 채력 회복
        }

    }

   
    public void  UsingItem()
    {
        ItemEffect((int)ItemNumber);
        Destroy(ItemInven[(int)ItemNumber]);
        ItemInven.RemoveAt((int)ItemNumber);
        InventoryItems.RemoveAt((int)ItemNumber);
        CurrentItemCount--;
        InitializedItem();
    }



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



    }
    public List<GameObject> ItemInven = new List<GameObject>(); // 아이템 목록
    [SerializeField] RenderTexture RenderText; // 렌더 텍스처
    [SerializeField] GameObject ItemUI; // 아이템 UI 프리팹
    [SerializeField] GameObject ItemUISpawnPos; // 아이템을 생성할 위치
    [SerializeField] RenderTexture RT; // 렌더 텍스처

    [SerializeField] GameObject Inventory; // ItemUI생성할 위치
    [SerializeField] GameObject ItemRawImage; //Item이미지

    public List<GameObject> InventoryItems = new List<GameObject>();
   
    void InitializedItem()
    {
        Transform inventoryTransform = Inventory.transform;

        // Inventory의 모든 하위 객체를 제거합니다.
        foreach (Transform child in inventoryTransform)
        {
            Destroy(child.gameObject);
        }
        InventoryItems = new List<GameObject>();
        InventoryItems.Clear();

        // 아이템 개수만큼 반복해서 아이템을 생성합니다.
        for (int i = 0; i < ItemInven.Count; i++)
        {
            GameObject fruit = Instantiate(ItemRawImage, inventoryTransform);
            fruit.GetComponent<RawImage>().texture = ItemInven[i].GetComponentInChildren<Camera>().targetTexture;
            InventoryItems.Add(fruit);
            // 아이템 위치 조정

            if (i == InventoryItems.Count - 1 && InventoryItems.Count >= 3)
            {
                fruit.transform.localPosition -= new Vector3(xOffset, 0, 0);

            }
            else
            {

                //float xOffset = i * 180; // 아이템 간격
                fruit.transform.localPosition += new Vector3(xOffset * i, 0, 0);

            }


        }
        if(InventoryItems.Count>0)
        {
        LeftX = InventoryItems[0].transform.localPosition.x - (xOffset + 1);
        //RightX = InventoryItems[InventoryItems.Count - 2].transform.localPosition.x + 1;

        if (InventoryItems.Count > 3)
        {
            RightX = InventoryItems[InventoryItems.Count - 2].transform.localPosition.x + (xOffset + 1);
        }
        else
        {
            RightX = InventoryItems[0].transform.localPosition.x + (xOffset + 1);

        }

        }

    }


    void SpawnItemUI(GameObject fruit)
    {
        // 아이템 UI를 생성하고 아이템 목록에 추가
        GameObject itemUIShow = Instantiate(ItemUI, ItemUISpawnPos.transform.position + new Vector3(3 * ItemInven.Count, 0, 0), Quaternion.identity);
        Instantiate(fruit, itemUIShow.transform);

        // 렌더 텍스처를 생성하고 카메라에 할당
        RT = Instantiate(RenderText);
        itemUIShow.GetComponentInChildren<Camera>().targetTexture = RT;

        ItemInven.Add(itemUIShow);
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
                    SpawnItemUI(SellingThing);
                    CurrentItemCount++;
                    InitializedItem();


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


    private float moveDuration = 0.05f; // 이동에 걸리는 시간 (조절 가능)
    private bool isMoving = false; // 이동 중 여부를 나타내는 변수

    [SerializeField] float ItemNumber = 0;
    public void RightMove()
    {
        if (InventoryItems.Count > 1)
        {
            if (!isMoving)
            {
                StartCoroutine(MoveRight(InventoryItems));
                ++ItemNumber;
                if (ItemNumber > InventoryItems.Count-1)
                {
                    ItemNumber = 0;
                }
            }
        }
    }

    public void LeftMove()
    {
        if (InventoryItems.Count > 1)
        {
            if (!isMoving)
            {
                StartCoroutine(MoveRowLeft(InventoryItems));
                --ItemNumber;
                if (ItemNumber-1 < 0)
                {
                    ItemNumber = InventoryItems.Count-1;
                }
            }
        }
    }


    float xOffset = 180;

    //오른쪽 스크롤 코루틴
    private IEnumerator MoveRight(List<GameObject> row)
    {
        isMoving = true; // 이동 중 플래그 설정

        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> endPositions = new List<Vector3>();

        // 시작 위치와 목표 위치 저장
        foreach (GameObject item in row)
        {
            startPositions.Add(item.transform.localPosition); // 시작 위치 저장
            endPositions.Add(item.transform.localPosition + new Vector3(xOffset, 0, 0)); // 목표 위치 저장
        }

        float elapsedTime = 0f;

        // 이동 시간동안 반복
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration; // 보간값 계산

            // 모든 객체에 대해 현재 위치를 보간하여 설정
            for (int i = 0; i < row.Count; i++)
            {
                row[i].transform.localPosition = Vector3.Lerp(startPositions[i], endPositions[i], t);
            }

            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 한 프레임을 기다립니다.
        }

        // 이동이 끝난 후 최종 위치 설정
        for (int i = 0; i < row.Count; i++)
        {
            row[i].transform.localPosition = endPositions[i];

            // 오른쪽 끝으로 넘어간 객체 처리
            if (row[i].transform.localPosition.x > RightX)
            {
                row[i].transform.localPosition -= new Vector3(xOffset * row.Count, 0, 0);
            }
        }

        isMoving = false; // 이동 종료 후 플래그 해제
    }


    //왼쪽 스크롤 코루틴
    private IEnumerator MoveRowLeft(List<GameObject> row)
    {
        isMoving = true; // 이동 중 플래그 설정

        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> endPositions = new List<Vector3>();

        // 시작 위치와 목표 위치 저장
        foreach (GameObject item in row)
        {
            startPositions.Add(item.transform.localPosition); // 시작 위치 저장
            endPositions.Add(item.transform.localPosition - new Vector3(xOffset, 0, 0)); // 목표 위치 저장 (왼쪽으로 이동)
        }

        float elapsedTime = 0f;

        // 이동 시간 동안 반복
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration; // 보간값 계산

            // 모든 객체에 대해 현재 위치를 보간하여 설정
            for (int i = 0; i < row.Count; i++)
            {
                row[i].transform.localPosition = Vector3.Lerp(startPositions[i], endPositions[i], t);
            }

            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 한 프레임을 기다립니다.
        }

        // 이동이 끝난 후 최종 위치 설정
        for (int i = 0; i < row.Count; i++)
        {
            row[i].transform.localPosition = endPositions[i];

            // 왼쪽 끝으로 넘어간 객체 처리
            if (row[i].transform.localPosition.x < LeftX)
            {
                row[i].transform.localPosition += new Vector3(xOffset * row.Count, 0, 0);
            }
        }

        isMoving = false; // 이동 종료 후 플래그 해제
    }


}