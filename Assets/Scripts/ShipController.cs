using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Camera cam;
    public Transform player;
    public Rigidbody rigidBody;

    Vector2 mousePos;
    Vector3 playerPos;

    // Update is called once per frame
    void Update()
    {
        // if these variables were assigned in FixedUpdate(), they would only be updated at fixed intervals, 
        // potentially causing jerky or delayed movement.
        mousePos = Input.mousePosition;
        playerPos = cam.WorldToScreenPoint(player.position);
    }

    // FixedUpdate is called fixed intervals determined by the physics system
    private void FixedUpdate()
    {
        // get the angle of the mouse in relation to the player
        // point the player in the direction of the mouse
        Vector2 pos;
        pos.x = playerPos.x;
        pos.y = playerPos.y;

        Vector2 lookDir = mousePos - pos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion newRotation = Quaternion.Euler(0, 0, angle);
        player.rotation = newRotation;


        if (Input.GetMouseButton(0))
        {
            rigidBody.AddForce(transform.right);
        }

    }
}
