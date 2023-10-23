using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{
    Transform currentTarget; // 현재 타겟
    Animator anim; // 애니메이터

    [SerializeField] LayerMask targetLayers; // 타겟 레이어 마스크
    [SerializeField] Transform enemyTarget_Locator; // 적 타겟 위치 지정

    [Tooltip("StateDrivenMethod for Switching Cameras")]
    [SerializeField] Animator cinemachineAnimator; // 시네머신 애니메이터

    [Header("Settings")]
    [SerializeField] bool zeroVert_Look; // 수직 방향을 고려한 타겟 봐야 하는지 여부
    [SerializeField] float noticeZone = 10; // 탐지 범위
    [SerializeField] float lookAtSmoothing = 2; // 봐야 하는 위치로 회전할 때의 부드러움 정도
    [Tooltip("Angle_Degree")][SerializeField] float maxNoticeAngle = 60; // 최대 탐지 각도
    [SerializeField] float crossHair_Scale = 0.1f; // 크로스헤어 크기 조절

    Transform cam; // 카메라의 트랜스폼
    bool enemyLocked; // 적 타겟 잠금 여부
    float currentYOffset; // 현재 Y 오프셋
    Vector3 pos; // 위치 벡터

    [SerializeField] CameraFollow camFollow; // 카메라 추적 스크립트
    [SerializeField] Transform lockOnCanvas; // 락온 표시용 캔버스
    DefMovement defMovement; // 캐릭터 이동 스크립트

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
        lockOnCanvas.gameObject.SetActive(false); // 락온 캔버스 비활성화
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
                    // 이미 타겟이 있는 경우, 리셋.
                    ResetTarget();
                    return;
                }

                if (currentTarget = ScanNearBy()) FoundTarget(); // 주변을 스캔하여 타겟을 찾음
                else ResetTarget(); // 타겟을 찾지 못한 경우 리셋
            }

            if (enemyLocked)
            {
                if (!TargetOnRange()) ResetTarget(); // 타겟이 범위를 벗어난 경우 리셋
                LookAtTarget(); // 타겟을 바라봄
            }
        }
    }

    void FoundTarget()
    {
        lockOnCanvas.gameObject.SetActive(true); // 락온 캔버스 활성화
        cinemachineAnimator.Play("TargetCamera"); // 시네머신 애니메이터의 TargetCamera 애니메이션 재생
        enemyLocked = true; // 적 타겟 잠금 상태로 변경
        anim.SetBool("LookOn", true);


    }

    void ResetTarget()
    {
        lockOnCanvas.gameObject.SetActive(false); // 락온 캔버스 비활성화
        currentTarget = null; // 현재 타겟 초기화
        enemyLocked = false; // 적 타겟 잠금 해제
        cinemachineAnimator.Play("FollowCamera"); // 시네머신 애니메이터의 FollowCamera 애니메이션 재생
        anim.SetBool("LookOn", false);


    }

    private Transform ScanNearBy()
    {
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers); // 주변의 타겟을 스캔
        float closestAngle = maxNoticeAngle; // 최소 각도 초기화
        Transform closestTarget = null; // 가장 가까운 타겟 초기화

        if (nearbyTargets.Length <= 0) return null; // 주변에 타겟이 없으면 null 반환

        for (int i = 0; i < nearbyTargets.Length; i++)
        {
            Vector3 dir = nearbyTargets[i].transform.position - cam.position; // 카메라 위치에서 타겟까지의 벡터 계산
            dir.y = 0; // 수직 방향 무시
            float _angle = Vector3.Angle(cam.forward, dir); // 카메라 앞쪽과의 각도 계산

            if (_angle < closestAngle)
            {
                closestTarget = nearbyTargets[i].transform;
                closestAngle = _angle;
            }
        }

        if (!closestTarget) return null; // 가장 가까운 타겟이 없으면 null 반환

        float h1 = closestTarget.GetComponent<CapsuleCollider>().height;
        float h2 = closestTarget.localScale.y;
        float h = h1 * h2;
        float half_h = (h / 2) / 2;
        currentYOffset = h - half_h;
        //  currentYOffset =  half_h;

        //if (zeroVert_Look && currentYOffset > 1.6f && currentYOffset < 1.6f * 3)
        //    currentYOffset = 1.6f;

        Vector3 tarPos = closestTarget.position + new Vector3(0, currentYOffset, 0);

        if (Blocked(tarPos)) return null; // 타겟이 가려진 경우 null 반환

        return closestTarget; // 가장 가까운 타겟 반환
    }

    bool Blocked(Vector3 t)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * 0.5f, t, out hit))
        {
            if (!hit.transform.CompareTag("Enemy")) return true; // 다른 물체가 가렸을 경우 true 반환
        }
        return false; // 가려지지 않은 경우 false 반환
    }

    bool TargetOnRange()
    {
        float dis = (transform.position - pos).magnitude; // 현재 위치에서 타겟까지 거리 계산
        if (dis / 2 > noticeZone) return false; // 거리가 탐지 범위의 절반보다 크면 false 반환
        else return true; // 아니면 true 반환
    }

    private void LookAtTarget()
    {
        if (currentTarget == null)
        {
            ResetTarget(); // 현재 타겟이 없으면 타겟 잠금을 해제하고 리턴
            return;
        }

        pos = currentTarget.position + new Vector3(0, currentYOffset, 0); // 타겟 위치에 Y 오프셋을 더하여 카메라 표시 위치 설정
        lockOnCanvas.position = pos; // 락온 캔버스 위치를 타겟 위치로 설정
        lockOnCanvas.localScale = Vector3.one * ((cam.position - pos).magnitude * crossHair_Scale); // 락온 캔버스 크기 조절

        enemyTarget_Locator.position = pos; // 적 타겟 위치 지정

        Vector3 dir = currentTarget.position - transform.position; // 카메라에서 타겟까지의 방향 벡터 계산
        dir.y = 0; // 수직 방향은 무시
        Quaternion rot = Quaternion.LookRotation(dir); // 타겟을 향한 회전을 계산
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * lookAtSmoothing); // 부드럽게 회전 적용
    }
    [SerializeField] float aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone); // 디버깅용으로 주변 탐지 범위를 시각적으로 나타냄
    }
}