using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] float turnSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        float r = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);
    }
}
