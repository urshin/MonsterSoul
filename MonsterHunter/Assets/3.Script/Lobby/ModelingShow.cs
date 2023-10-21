using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelingShow : MonoBehaviour
{
    [SerializeField] GameObject RotateObject;
    [SerializeField] GameObject SpawnPos;
    [SerializeField] float RotateSpeed;
    public GameObject GetWeapon;
    public Camera cam;
    // Update is called once per frame
    bool isSpawn;
    private void Start()
    {
        isSpawn = false;
       

    }
    void Update()
    {
        if(GetWeapon !=null && !isSpawn)
        {
            GameObject Weapon = Instantiate(GetWeapon, SpawnPos.transform);
            Weapon.GetComponent<Rigidbody>().useGravity = false;
            


            isSpawn= true;
        }
        // y축을 기준으로 회전합니다.
        RotateObject.transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);
    }
}