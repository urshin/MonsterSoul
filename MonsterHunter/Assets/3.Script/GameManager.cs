using System.Collections;
using System.Collections.Generic;
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

   // public PlayerState PlayerCurrentState;





}
