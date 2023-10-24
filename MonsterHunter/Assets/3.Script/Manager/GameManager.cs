using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

//public enum PlayerState
//{
//    Move,
//    Attack,
//    Jump,
//    Roll,
//}

public class GameManager : MonoBehaviour
{
    //어디에서나 접근 할 수 있도록 static으로 할당 싱글톤
    public static GameManager Instance;
    public void Awake()
    {
        if (Instance == null) //정적으로 자신을 체크함, null인진
        {
            Instance = this; //이후 자기 자신을 저장함.
            DontDestroyOnLoad(gameObject);// 씬이 바뀌어도 삭제 안됨
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

    public AnimatorController Worrior;

    [Header("Load")]
    public bool IsLoading;
    public bool PlayerLoad;
    public bool SandWormLoad;

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
    }


    
}
