using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera1;
    public CinemachineFreeLook virtualCamera2;

    private void Update()
    {
        // 특정 키 또는 이벤트(예: 플레이어가 어떤 지점에 도달하는 등)에 따라 카메라 전환을 수행
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        // 현재 활성화된 Virtual Camera를 비활성화하고 다른 Virtual Camera를 활성화
        if (virtualCamera1.Priority > virtualCamera2.m_Priority)
        {
            virtualCamera1.Priority = 10; // 현재 활성화된 카메라의 Priority 값을 기본값(예: 10)으로 설정
            virtualCamera2.m_Priority = 11; // 다른 카메라의 Priority 값을 높임
        }
        else
        {
            virtualCamera2.m_Priority = 10;
            virtualCamera1.Priority = 11;
        }
    }
}
