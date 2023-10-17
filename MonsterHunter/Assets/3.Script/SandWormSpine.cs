using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormSpine : MonoBehaviour
{
    public SandWorm sandWorm; //»÷µå¿ú ÂüÁ¶

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

}
