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




    private void Start()
    {
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

    public List<GameObject> ItemList = new List<GameObject>();

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
        BuyStore();

        if(Input.GetKeyDown (KeyCode.P))
        {
            ItemInvenUI.Clear();
            CurrentItemCount = 0;
        }

    }
    public List<GameObject> ItemInven = new List<GameObject>(); // ������ ���
    public List<GameObject> ItemInvenUI = new List<GameObject>(); // ������ UI ���
    [SerializeField] RenderTexture RenderText; // ���� �ؽ�ó
    [SerializeField] GameObject ItemUI; // ������ UI ������
    [SerializeField] GameObject ItemUISpawnPos; // ������ UI�� ������ ��ġ
    [SerializeField] RenderTexture RT; // ���� �ؽ�ó
    [SerializeField] GameObject Inventory; // �κ��丮 ������Ʈ
    [SerializeField] GameObject rawimage; // RawImage ������Ʈ

    void SpawnItemUI(GameObject fruit)
    {
        // ������ UI�� �����ϰ� ������ ��Ͽ� �߰�
        GameObject itemUIShow = Instantiate(ItemUI, ItemUISpawnPos.transform.position + new Vector3(3 * ItemInven.Count, 0, 0), Quaternion.identity);
        Instantiate(fruit, itemUIShow.transform);

        // ���� �ؽ�ó�� �����ϰ� ī�޶� �Ҵ�
        RT = Instantiate(RenderText);
        itemUIShow.GetComponentInChildren<Camera>().targetTexture = RT;

        // RawImage�� �����ϰ� ���� �ؽ�ó�� �Ҵ��� �� UI ��Ͽ� �߰�
        GameObject RI = Instantiate(rawimage, Inventory.transform);
        RI.GetComponent<RawImage>().texture = RT;

        ItemInvenUI.Add(RI);
        ItemInven.Add(itemUIShow);
        initializedItemUI();
    }

    void initializedItemUI()
    {
        float spacing = 180f; // �� ������ ������ ����
        for (int i = 0; i < ItemInvenUI.Count; i++)
        {
            // ������ UI�� ��ġ�� ����
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

            // E Ű�� ������ �� �������� �����ϰ� UI�� �߰�
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
            // ������ �Ÿ��� �� ��� ���� UI ��Ȱ��ȭ
            if (StoreCanvas != null)
            {
                StoreCanvas.SetActive(false);
            }
        }
    }


}