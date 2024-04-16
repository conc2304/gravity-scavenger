using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    // Laser Gun Variabls
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 1f)]
    [SerializeField] private float fireRate = 0.5f;
    private float fireTimer;

    // Controller Variables
    public Camera cam;
    public Transform player;
    public Rigidbody rigidBody;
    public float thrust = 10f;

    Vector2 mousePos;
    Vector3 playerPos;

    // Update is called once per frame
    void Update()
    {
        // if these variables were assigned in FixedUpdate(), they would only be updated at fixed intervals, 
        // potentially causing jerky or delayed movement.
        mousePos = Input.mousePosition;
        playerPos = cam.WorldToScreenPoint(player.position);

        // left mouse button to fire lasers
        if (Input.GetMouseButton(1) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }
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
            rigidBody.AddForce(transform.right * thrust * Time.deltaTime);
        }
    }

    private void Shoot()
    {
        Instantiate(laserPrefab, firingPoint.position, firingPoint.rotation);
    }
}
