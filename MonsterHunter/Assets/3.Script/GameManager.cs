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

   // public PlayerState PlayerCurrentState;





}
