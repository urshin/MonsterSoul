using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float Attack;

    [SerializeField] GameObject[] SandWormEffect;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Player.Instance.TotalDamage();
        }
    }
    private bool canSpawnEffect = true;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && canSpawnEffect)
        {
            // 충돌된 지점
            Vector3 spawnPosition = other.ClosestPointOnBounds(transform.position);

            // 충돌된 지점과 적 오브젝트를 바라보는 방향
            Vector3 direction = (transform.position - spawnPosition).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);

            // SandWormEffect 0.3초에 한 번씩 생성
            StartCoroutine(SpawnEffect(spawnPosition, rotation));
        }
    }

    private IEnumerator SpawnEffect(Vector3 spawnPosition, Quaternion rotation)
    {
        canSpawnEffect = false;
        for (int i = 0; i < SandWormEffect.Length; i++)
        {
            GameObject effect = Instantiate(SandWormEffect[i], spawnPosition, rotation);
            Destroy(effect, 0.5f);
        }

        yield return new WaitForSeconds(0.1f);
        canSpawnEffect = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
