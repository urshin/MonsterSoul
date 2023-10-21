using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting_Button : MonoBehaviour
{
    private Button button;
    private Slider slider;

    LobbyManager lobby;

    void Start()
    {
        lobby = LobbyManager.Instance; //로비메니저 연결
        if (gameObject.GetComponent<Button>() != null)
        {
            button = GetComponent<Button>(); //버튼 component 가져오기
            button.onClick.AddListener(Click); //인자가 없을 때 함수 호출

        }
        if(gameObject.GetComponent<Slider>() != null)
        {
            slider = GetComponent<Slider>();
            
        }
    }

    private void Update()
    {
        switch(gameObject.name)
        {
            case "MouseSensitivity_Slider":
                GameManager.Instance.MouseSensitivity = slider.value;
                break;
            case "Master_Slider":
                GameManager.Instance.Sound_Master = slider.value;
                break;
            case "BGM_Slider":
                GameManager.Instance.Sound_BGM = slider.value;
                break;
            case "Effect_Slider":
                GameManager.Instance.Sound_Effect = slider.value;
                break;
                
        }
    }


    void Click() //클릭하는 버튼에 따라 다른 기능 구현
    {
        switch (gameObject.name)
        {
            case "Back":
                lobby.SetOff(lobby.Setting);
                lobby.SetOn(lobby.First);
                break;
            case "Low":
                GameManager.Instance.InGameGraphic = GameManager.Graphic.Low;
                break;
            case "Middle":
                GameManager.Instance.InGameGraphic = GameManager.Graphic.Middle;
                break;
            case "High":
                GameManager.Instance.InGameGraphic = GameManager.Graphic.High;
                break;

        }
    }
}
