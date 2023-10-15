using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    Animator anim;
    public bool ComboPossible;
    


    private void Start()
    {
        anim = GetComponent<Animator>();
        ComboPossible = false;
    }

    private void Update()
    {
       
    }


    void WeaponCollider_True ()
    {
        Player.Instance.CurrentWeapon.GetComponent<CapsuleCollider>().enabled = true;
    }
    void WeaponCollider_False()
    {
        Player.Instance.CurrentWeapon.GetComponent<CapsuleCollider>().enabled = false;
    }

  

    void OffNextAttack()
    {
        anim.SetBool("GoNextAttack", false);
    }
    void ture_AbleCombo()
    {
        anim.SetBool("AbleCombo", true);
    }
    void False_AbleCombo()
    {
        anim.SetBool("AbleCombo", false);
    }
    void IsAttack_True()
    {
        anim.SetBool("IsAttack",true);
    }
    void IsAttack_False()
    {
        anim.SetBool("IsAttack", false);
    }
}
