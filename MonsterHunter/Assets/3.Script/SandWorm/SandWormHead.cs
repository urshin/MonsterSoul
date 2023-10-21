using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormHead : MonoBehaviour
{
    [SerializeField] GameObject[] Rock;
    [SerializeField] GameObject[] GroundEffect;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            ShootingFire();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            ShootingFire();
        }
    }

    private void SpawnGroundEffect(Collider other)
    {
        Vector3 spawnPosition = other.ClosestPointOnBounds(transform.position);

        // 충돌된 지점과 적 오브젝트를 바라보는 방향
        Vector3 direction = (transform.position - spawnPosition).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        for (int i = 0; i < Rock.Length; i++)
        {
        GameObject effect = Instantiate(GroundEffect[i], spawnPosition, rotation);
        Destroy(effect, 0.5f);

        }
        // SandWormEffect 0.3초에 한 번씩 생성
    }

    void ShootingFire()
    {
        for (int i = 0; i < Rock.Length; i++)
        {
            GameObject rock = Instantiate(Rock[i],transform.position, Quaternion.identity);
            Destroy(rock, 2f);
        }
    }
 
}
