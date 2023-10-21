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
        lobby = LobbyManager.Instance; //�κ�޴��� ����
        if (gameObject.GetComponent<Button>() != null)
        {
            button = GetComponent<Button>(); //��ư component ��������
            button.onClick.AddListener(Click); //���ڰ� ���� �� �Լ� ȣ��

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


    void Click() //Ŭ���ϴ� ��ư�� ���� �ٸ� ��� ����
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
