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
                InGameManager.Instance.TheWorld(InGameManager.Instance.PausePopUp);
                GameManager.Instance.cursorLocked = false;

                break;
            case "Exit":
                SceneManager.LoadScene("Lobby");
                break;
            case "Restart":
                string name = GameManager.Instance.CurrentBoss.name.Trim();
                SceneManager.LoadScene(name);
                 break;
               


        }
    }



}
