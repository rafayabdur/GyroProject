using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroMove2 : MonoBehaviour
{
    Gyroscope steerGyro;
    private Rigidbody rb;
    public float Speed = 2f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        steerGyro = Input.gyro;
        steerGyro.enabled = true;
            
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = steerGyro.attitude;
        float X = steerGyro.attitude.x;
        float Z = steerGyro.attitude.z;
        //rb.AddForce(X * Speed * Time.deltaTime, 0f ,Z * Speed * Time.deltaTime);
        rb.velocity = new Vector3(X * Speed * Time.deltaTime, 0f, Z * Speed * Time.deltaTime);
     }
}
