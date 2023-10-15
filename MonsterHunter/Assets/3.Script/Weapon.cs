using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float Attack;




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Player.Instance.TotalDamage();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
