using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormSpine : MonoBehaviour
{
    public SandWormBoss sandWorm; //»÷µå¿ú ÂüÁ¶

    public GameObject GroundEffect;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            if (!sandWorm.IsBulling)
            {
                int randomnum = Random.Range(0, 4);
                    switch (randomnum)
                {
                    case 0:
                        SoundManager.Instance.PlayEffect("SandWormDamage1");
                        break;
                    case 1:
                        SoundManager.Instance.PlayEffect("SandWormDamage2");
                        break;
                    case 2:
                        SoundManager.Instance.PlayEffect("SandWormDamage3");
                        break;
                    case 3:
                        SoundManager.Instance.PlayEffect("SandWormDamage4");
                        break;
                }
               

                sandWorm.SandWormHP -= Player.Instance.PlayerTotalDamage();
                Debug.Log(Player.Instance.PlayerTotalDamage());
            }
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
