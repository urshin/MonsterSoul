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
        button.onClick.AddListener(Click); //인자가 없을 때 함수 호출
    }
    void Click() //클릭하는 버튼에 따라 다른 기능 구현
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
