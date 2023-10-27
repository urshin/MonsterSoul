using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//public enum PlayerState
//{
//    Move,
//    Attack,
//    Jump,
//    Roll,
//}

public class GameManager : MonoBehaviour
{
    //��𿡼��� ���� �� �� �ֵ��� static���� �Ҵ� �̱���
    public static GameManager Instance;
    public void Awake()
    {
        if (Instance == null) //�������� �ڽ��� üũ��, null����
        {
            Instance = this; //���� �ڱ� �ڽ��� ������.
            DontDestroyOnLoad(gameObject);// ���� �ٲ� ���� �ȵ�
        }
    }
    public enum Graphic
    {
        Low,
        Middle,
        High,
    }
    
    public bool IsBossRoomEnter;
    [Header("GameData")]
    public GameObject CurrentPlayerCharactor;
    public GameObject CurrentWeapon;
    public GameObject CurrentBoss;

    [Header("Player Info")]
    //public GameObject Weapon;
    public float PlayerHp;
    public float PlayerSpeed;
    public float PlayerAttack;
    public float PlayerStamina;
    public float PlayerJumpPower;
    public float PlayerMaximumItem;


    [Header("Setting")]
    public Graphic InGameGraphic;
    public float MouseSensitivity;
    public float Sound_Master;
    public float Sound_BGM;
    public float Sound_Effect;
    public List<GameObject> PlayerCharactor = new List<GameObject>();
    public List<GameObject> WeaponList = new List<GameObject>();
    public List<GameObject> BossList = new List<GameObject>();
    public bool cursorLocked = false; // Ŀ�� ��� ���� ����

    public AnimatorController Worrior;

    [Header("Load")]
    public bool IsLoading;
    public bool PlayerLoad;
    public bool SandWormLoad;


    [Header("PopUp")]
    public GameObject Message;

    private void Start()
    {
        MouseSensitivity = 0.6f;
    }

    private void Update()
    {
        if(PlayerLoad&&SandWormLoad)
        {
            IsLoading = false;
        }
        else
        {
            IsLoading = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MakeCursorLocked();
        }
            if (cursorLocked)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false; 
                Cursor.lockState = CursorLockMode.Locked;
            }
    }

    public void MakeCursorLocked()
    {
        cursorLocked = !cursorLocked;
    }
    public void SpawnMessage(string thing)
    {
        GameObject Canvas = GameObject.Find("GUI");
        GameObject PopUp =  Instantiate(Message, Canvas.transform);
        PopUp.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Text>().text = thing;
    }
    public IEnumerator Restart(string name)
    {
        SceneManager.LoadScene("Lobby");
        yield return new WaitForSecondsRealtime(0.1f);
        SceneManager.LoadScene(name);
    }
}
