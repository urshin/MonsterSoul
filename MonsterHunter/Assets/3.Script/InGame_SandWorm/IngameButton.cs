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
        button = GetComponent<Button>(); //버튼 component 가져오기
        button.onClick.AddListener(Click); //인자가 없을 때 함수 호출

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
