using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameButton : MonoBehaviour
{
    private Button button;


 

    void Start()
    {
        button = GetComponent<Button>(); //��ư component ��������
        button.onClick.AddListener(Click); //���ڰ� ���� �� �Լ� ȣ��

    }



    void Click()
    {
        SoundManager.Instance.PlayEffect("BTN");

        switch (gameObject.name)
        {
            case "Back":
            case "Continue":
                InGameManager.Instance.TheWorld();
                break;
            case "Exit":
                SceneManager.LoadScene("Lobby");
                break;
        }
    }



}
