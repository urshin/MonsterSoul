using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormSpine : MonoBehaviour
{
    public SandWormBoss sandWorm; //»÷µå¿ú ÂüÁ¶

    private void OnTriggerEnter(Collider other)
    {
        if(!sandWorm.IsBulling)
        sandWorm.SandWormHP -= Player.Instance.PlayerTotalDamage();
    }

    private void OnTriggerStay(Collider other)
    {
        sandWorm.IsBulling = true;
    }
    private void OnTriggerExit(Collider other)
    {
        sandWorm.IsBulling = false;
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
