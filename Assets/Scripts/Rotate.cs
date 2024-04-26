using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeedX = 0f;
    public float rotationSpeedY = 0f;
    public float rotationSpeedZ = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime, Space.Self);
    }
}
