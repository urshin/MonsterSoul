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
    public float playerOriginHP; // �÷��̾��� ����ü��
    public float PlayerSpeed; // �÷��̾��� �̵� �ӵ�
    public float PlayerAttack; // �÷��̾��� ���ݷ�
    public float PlayerrStamina; // �÷��̾��� ���׹̳�
    public float PlayerMotionDamage; // �÷��̾��� ��� ������
    public float PlayerMaximumItem;
    public float CurrentItemCount;
    GameObject WeaponScript; // ���� ��ũ��Ʈ�� ������ ����

    [SerializeField] GameObject PlayerHP_Bar;
    [SerializeField] GameObject PlayerrStamina_Bar;

    // public GameObject Weapon; // ���� ������
    public GameObject CurrentWeapon; // ���� ������ ���⸦ ������ ����

    public bool isPlayerBulling;

    [SerializeField] GameObject WeaponPos_r; // ���⸦ ��ȯ�� ��ġ (������ �� ��ġ)
    public GameObject PlayerAvatar; //�ƹ�Ÿ

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
        isPlayerBulling = false;//���� ������ ����
        
    }


    void LateStart()
    {
        SpawnPlayerAvatar();
        SpawnWeapon(); // ���⸦ �����ϴ� �Լ� ȣ��
        WeaponScript = GameObject.FindGameObjectWithTag("Weapon"); // "Weapon" �±׸� ���� ������Ʈ�� ã�� WeaponScript ������ �Ҵ�
        CurrentWeapon = GameObject.FindGameObjectWithTag("Weapon"); // "Weapon" �±׸� ���� ������Ʈ�� ���� ����� ����\
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
                Debug.Log("�� ����");
                LeftMove();
            }
            else if (wheelInput < 0)
            {
                // ���� ��� �÷��� ���� ó�� ��
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
        // �θ�-�ڽ� ���� ����
        PlayerAvatar.transform.parent = gameObject.transform;
        PlayerAvatar.GetComponent<Animator>().runtimeAnimatorController = GM.Worrior;
        anime = PlayerAvatar.GetComponent<Animator>();
    }
    void SpawnWeapon()
    {
        WeaponPos_r = GameObject.FindGameObjectWithTag("WeaponPos_R");
        Instantiate(GM.CurrentWeapon, WeaponPos_r.transform); // Weapon �������� WeaponPos_r ��ġ�� ����
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
                Debug.Log("�����溸");
                PlayerHP -= SandWormBoss.Instance.SandWormAttackDamage;
                SoundManager.Instance.PlayEffect("GotDamage");

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

    void ItemEffect(int Itemnum)
    {
        Debug.Log(ItemInven[Itemnum].transform.GetChild(1).gameObject.name);
        string itemname = ItemInven[Itemnum].transform.GetChild(1).name.Replace("(Clone)", "").Trim();
        switch (itemname)
        {
            case "Banana":
                Debug.Log(itemname + "ȿ�� �ߵ�!!");
                EffectOn(PlayerEffect.Buff);
                break;
            case "Orange":
                Debug.Log(itemname + "ȿ�� �ߵ�!!");
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
            PlayerHP += playerOriginHP / 4; //�÷��̾� ä�� ȸ��
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



    [SerializeField] float Radius;//���� ��ó ����
    [SerializeField] LayerMask targetLayers; // Ÿ�� ���̾� ����ũ ���߿� �÷��̾�� ����
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
    public List<GameObject> ItemInven = new List<GameObject>(); // ������ ���
    [SerializeField] RenderTexture RenderText; // ���� �ؽ�ó
    [SerializeField] GameObject ItemUI; // ������ UI ������
    [SerializeField] GameObject ItemUISpawnPos; // �������� ������ ��ġ
    [SerializeField] RenderTexture RT; // ���� �ؽ�ó

    [SerializeField] GameObject Inventory; // ItemUI������ ��ġ
    [SerializeField] GameObject ItemRawImage; //Item�̹���

    public List<GameObject> InventoryItems = new List<GameObject>();
   
    void InitializedItem()
    {
        Transform inventoryTransform = Inventory.transform;

        // Inventory�� ��� ���� ��ü�� �����մϴ�.
        foreach (Transform child in inventoryTransform)
        {
            Destroy(child.gameObject);
        }
        InventoryItems = new List<GameObject>();
        InventoryItems.Clear();

        // ������ ������ŭ �ݺ��ؼ� �������� �����մϴ�.
        for (int i = 0; i < ItemInven.Count; i++)
        {
            GameObject fruit = Instantiate(ItemRawImage, inventoryTransform);
            fruit.GetComponent<RawImage>().texture = ItemInven[i].GetComponentInChildren<Camera>().targetTexture;
            InventoryItems.Add(fruit);
            // ������ ��ġ ����

            if (i == InventoryItems.Count - 1 && InventoryItems.Count >= 3)
            {
                fruit.transform.localPosition -= new Vector3(xOffset, 0, 0);

            }
            else
            {

                //float xOffset = i * 180; // ������ ����
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
        // ������ UI�� �����ϰ� ������ ��Ͽ� �߰�
        GameObject itemUIShow = Instantiate(ItemUI, ItemUISpawnPos.transform.position + new Vector3(3 * ItemInven.Count, 0, 0), Quaternion.identity);
        Instantiate(fruit, itemUIShow.transform);

        // ���� �ؽ�ó�� �����ϰ� ī�޶� �Ҵ�
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

            // E Ű�� ������ �� �������� �����ϰ� UI�� �߰�
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
            // ������ �Ÿ��� �� ��� ���� UI ��Ȱ��ȭ
            if (StoreCanvas != null)
            {
                StoreCanvas.SetActive(false);
            }
        }
    }


    private float moveDuration = 0.05f; // �̵��� �ɸ��� �ð� (���� ����)
    private bool isMoving = false; // �̵� �� ���θ� ��Ÿ���� ����

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

    //������ ��ũ�� �ڷ�ƾ
    private IEnumerator MoveRight(List<GameObject> row)
    {
        isMoving = true; // �̵� �� �÷��� ����

        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> endPositions = new List<Vector3>();

        // ���� ��ġ�� ��ǥ ��ġ ����
        foreach (GameObject item in row)
        {
            startPositions.Add(item.transform.localPosition); // ���� ��ġ ����
            endPositions.Add(item.transform.localPosition + new Vector3(xOffset, 0, 0)); // ��ǥ ��ġ ����
        }

        float elapsedTime = 0f;

        // �̵� �ð����� �ݺ�
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration; // ������ ���

            // ��� ��ü�� ���� ���� ��ġ�� �����Ͽ� ����
            for (int i = 0; i < row.Count; i++)
            {
                row[i].transform.localPosition = Vector3.Lerp(startPositions[i], endPositions[i], t);
            }

            elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // �� �������� ��ٸ��ϴ�.
        }

        // �̵��� ���� �� ���� ��ġ ����
        for (int i = 0; i < row.Count; i++)
        {
            row[i].transform.localPosition = endPositions[i];

            // ������ ������ �Ѿ ��ü ó��
            if (row[i].transform.localPosition.x > RightX)
            {
                row[i].transform.localPosition -= new Vector3(xOffset * row.Count, 0, 0);
            }
        }

        isMoving = false; // �̵� ���� �� �÷��� ����
    }


    //���� ��ũ�� �ڷ�ƾ
    private IEnumerator MoveRowLeft(List<GameObject> row)
    {
        isMoving = true; // �̵� �� �÷��� ����

        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> endPositions = new List<Vector3>();

        // ���� ��ġ�� ��ǥ ��ġ ����
        foreach (GameObject item in row)
        {
            startPositions.Add(item.transform.localPosition); // ���� ��ġ ����
            endPositions.Add(item.transform.localPosition - new Vector3(xOffset, 0, 0)); // ��ǥ ��ġ ���� (�������� �̵�)
        }

        float elapsedTime = 0f;

        // �̵� �ð� ���� �ݺ�
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration; // ������ ���

            // ��� ��ü�� ���� ���� ��ġ�� �����Ͽ� ����
            for (int i = 0; i < row.Count; i++)
            {
                row[i].transform.localPosition = Vector3.Lerp(startPositions[i], endPositions[i], t);
            }

            elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // �� �������� ��ٸ��ϴ�.
        }

        // �̵��� ���� �� ���� ��ġ ����
        for (int i = 0; i < row.Count; i++)
        {
            row[i].transform.localPosition = endPositions[i];

            // ���� ������ �Ѿ ��ü ó��
            if (row[i].transform.localPosition.x < LeftX)
            {
                row[i].transform.localPosition += new Vector3(xOffset * row.Count, 0, 0);
            }
        }

        isMoving = false; // �̵� ���� �� �÷��� ����
    }


}