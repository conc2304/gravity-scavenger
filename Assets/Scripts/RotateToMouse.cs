using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Camera cam;
    public Transform player;


    // Vector2 movement;
    Vector2 mousePos;

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
    }

    private void FixedUpdate()
    {

        Vector2 pos;

        pos.x = 0;
        pos.y = 0;

        // print("RB: " + pos.x + ", " + pos.y);
        Vector2 lookDir = mousePos - pos;
        // float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion newRotation = Quaternion.Euler(0, 0, angle);
        player.rotation = newRotation;

    }
}
