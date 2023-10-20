using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormHead : MonoBehaviour
{
    [SerializeField] GameObject[] Rock;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            //Debug.Log(transform.position);
            ShootingFire();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            //Debug.Log(transform.position);
            ShootingFire();
        }
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
