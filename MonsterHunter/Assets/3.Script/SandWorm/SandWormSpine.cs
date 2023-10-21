using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormSpine : MonoBehaviour
{
    public SandWormBoss sandWorm; //»÷µå¿ú ÂüÁ¶

    public GameObject GroundEffect;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Weapon"))
        {
        if(!sandWorm.IsBulling)
        sandWorm.SandWormHP -= Player.Instance.PlayerTotalDamage();

        }
     
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            sandWorm.IsBulling = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            sandWorm.IsBulling = false;
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name+"enter");
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name + "exit");

    //}

}
