using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class Pop_Up : MonoBehaviour
{
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(Click); //���ڰ� ���� �� �Լ� ȣ��
    }
    void Click() //Ŭ���ϴ� ��ư�� ���� �ٸ� ��� ����
    {
        SoundManager.Instance.PlayEffect("BTN");

        switch (button.gameObject.name)
        {
            case "Back":
                Destroy(gameObject);
                break;



        }
    }
        // Update is called once per frame
        void Update()
    {
        
    }
}
