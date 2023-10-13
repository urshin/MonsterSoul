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
        // Ư�� Ű �Ǵ� �̺�Ʈ(��: �÷��̾ � ������ �����ϴ� ��)�� ���� ī�޶� ��ȯ�� ����
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        // ���� Ȱ��ȭ�� Virtual Camera�� ��Ȱ��ȭ�ϰ� �ٸ� Virtual Camera�� Ȱ��ȭ
        if (virtualCamera1.Priority > virtualCamera2.m_Priority)
        {
            virtualCamera1.Priority = 10; // ���� Ȱ��ȭ�� ī�޶��� Priority ���� �⺻��(��: 10)���� ����
            virtualCamera2.m_Priority = 11; // �ٸ� ī�޶��� Priority ���� ����
        }
        else
        {
            virtualCamera2.m_Priority = 10;
            virtualCamera1.Priority = 11;
        }
    }
}
