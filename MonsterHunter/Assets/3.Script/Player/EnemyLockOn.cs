using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{
    Transform currentTarget; // ���� Ÿ��
    Animator anim; // �ִϸ�����

    [SerializeField] LayerMask targetLayers; // Ÿ�� ���̾� ����ũ
    [SerializeField] Transform enemyTarget_Locator; // �� Ÿ�� ��ġ ����

    [Tooltip("StateDrivenMethod for Switching Cameras")]
    [SerializeField] Animator cinemachineAnimator; // �ó׸ӽ� �ִϸ�����

    [Header("Settings")]
    [SerializeField] bool zeroVert_Look; // ���� ������ ����� Ÿ�� ���� �ϴ��� ����
    [SerializeField] float noticeZone = 10; // Ž�� ����
    [SerializeField] float lookAtSmoothing = 2; // ���� �ϴ� ��ġ�� ȸ���� ���� �ε巯�� ����
    [Tooltip("Angle_Degree")][SerializeField] float maxNoticeAngle = 60; // �ִ� Ž�� ����
    [SerializeField] float crossHair_Scale = 0.1f; // ũ�ν���� ũ�� ����

    Transform cam; // ī�޶��� Ʈ������
    bool enemyLocked; // �� Ÿ�� ��� ����
    float currentYOffset; // ���� Y ������
    Vector3 pos; // ��ġ ����

    [SerializeField] CameraFollow camFollow; // ī�޶� ���� ��ũ��Ʈ
    [SerializeField] Transform lockOnCanvas; // ���� ǥ�ÿ� ĵ����
    DefMovement defMovement; // ĳ���� �̵� ��ũ��Ʈ

    void Start()
    {
        Invoke("LateStart", 0.1f);
    }

    void LateStart()
    {
        defMovement = GetComponent<DefMovement>();
        // anim = GetComponentInChildren<Animator>();
        //anim = Player.Instance.anime;
        cam = Camera.main.transform;
        lockOnCanvas.gameObject.SetActive(false); // ���� ĵ���� ��Ȱ��ȭ
    }

    void Update()
    {
        if(anim ==null)
        {
            anim = Player.Instance.anime;  
        }
        if (!GameManager.Instance.IsLoading)
        {

            camFollow.lockedTarget = enemyLocked;
            defMovement.lockMovement = enemyLocked;

            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                if (currentTarget)
                {
                    // �̹� Ÿ���� �ִ� ���, ����.
                    ResetTarget();
                    return;
                }

                if (currentTarget = ScanNearBy()) FoundTarget(); // �ֺ��� ��ĵ�Ͽ� Ÿ���� ã��
                else ResetTarget(); // Ÿ���� ã�� ���� ��� ����
            }

            if (enemyLocked)
            {
                if (!TargetOnRange()) ResetTarget(); // Ÿ���� ������ ��� ��� ����
                LookAtTarget(); // Ÿ���� �ٶ�
            }
        }
    }

    void FoundTarget()
    {
        lockOnCanvas.gameObject.SetActive(true); // ���� ĵ���� Ȱ��ȭ
        cinemachineAnimator.Play("TargetCamera"); // �ó׸ӽ� �ִϸ������� TargetCamera �ִϸ��̼� ���
        enemyLocked = true; // �� Ÿ�� ��� ���·� ����
        anim.SetBool("LookOn", true);


    }

    void ResetTarget()
    {
        lockOnCanvas.gameObject.SetActive(false); // ���� ĵ���� ��Ȱ��ȭ
        currentTarget = null; // ���� Ÿ�� �ʱ�ȭ
        enemyLocked = false; // �� Ÿ�� ��� ����
        cinemachineAnimator.Play("FollowCamera"); // �ó׸ӽ� �ִϸ������� FollowCamera �ִϸ��̼� ���
        anim.SetBool("LookOn", false);


    }

    private Transform ScanNearBy()
    {
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers); // �ֺ��� Ÿ���� ��ĵ
        float closestAngle = maxNoticeAngle; // �ּ� ���� �ʱ�ȭ
        Transform closestTarget = null; // ���� ����� Ÿ�� �ʱ�ȭ

        if (nearbyTargets.Length <= 0) return null; // �ֺ��� Ÿ���� ������ null ��ȯ

        for (int i = 0; i < nearbyTargets.Length; i++)
        {
            Vector3 dir = nearbyTargets[i].transform.position - cam.position; // ī�޶� ��ġ���� Ÿ�ٱ����� ���� ���
            dir.y = 0; // ���� ���� ����
            float _angle = Vector3.Angle(cam.forward, dir); // ī�޶� ���ʰ��� ���� ���

            if (_angle < closestAngle)
            {
                closestTarget = nearbyTargets[i].transform;
                closestAngle = _angle;
            }
        }

        if (!closestTarget) return null; // ���� ����� Ÿ���� ������ null ��ȯ

        float h1 = closestTarget.GetComponent<CapsuleCollider>().height;
        float h2 = closestTarget.localScale.y;
        float h = h1 * h2;
        float half_h = (h / 2) / 2;
        currentYOffset = h - half_h;
        //  currentYOffset =  half_h;

        //if (zeroVert_Look && currentYOffset > 1.6f && currentYOffset < 1.6f * 3)
        //    currentYOffset = 1.6f;

        Vector3 tarPos = closestTarget.position + new Vector3(0, currentYOffset, 0);

        if (Blocked(tarPos)) return null; // Ÿ���� ������ ��� null ��ȯ

        return closestTarget; // ���� ����� Ÿ�� ��ȯ
    }

    bool Blocked(Vector3 t)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * 0.5f, t, out hit))
        {
            if (!hit.transform.CompareTag("Enemy")) return true; // �ٸ� ��ü�� ������ ��� true ��ȯ
        }
        return false; // �������� ���� ��� false ��ȯ
    }

    bool TargetOnRange()
    {
        float dis = (transform.position - pos).magnitude; // ���� ��ġ���� Ÿ�ٱ��� �Ÿ� ���
        if (dis / 2 > noticeZone) return false; // �Ÿ��� Ž�� ������ ���ݺ��� ũ�� false ��ȯ
        else return true; // �ƴϸ� true ��ȯ
    }

    private void LookAtTarget()
    {
        if (currentTarget == null)
        {
            ResetTarget(); // ���� Ÿ���� ������ Ÿ�� ����� �����ϰ� ����
            return;
        }

        pos = currentTarget.position + new Vector3(0, currentYOffset, 0); // Ÿ�� ��ġ�� Y �������� ���Ͽ� ī�޶� ǥ�� ��ġ ����
        lockOnCanvas.position = pos; // ���� ĵ���� ��ġ�� Ÿ�� ��ġ�� ����
        lockOnCanvas.localScale = Vector3.one * ((cam.position - pos).magnitude * crossHair_Scale); // ���� ĵ���� ũ�� ����

        enemyTarget_Locator.position = pos; // �� Ÿ�� ��ġ ����

        Vector3 dir = currentTarget.position - transform.position; // ī�޶󿡼� Ÿ�ٱ����� ���� ���� ���
        dir.y = 0; // ���� ������ ����
        Quaternion rot = Quaternion.LookRotation(dir); // Ÿ���� ���� ȸ���� ���
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * lookAtSmoothing); // �ε巴�� ȸ�� ����
    }
    [SerializeField] float aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone); // ���������� �ֺ� Ž�� ������ �ð������� ��Ÿ��
    }
}