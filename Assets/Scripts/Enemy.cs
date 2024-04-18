using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Transform target;
    public float thrust = 3f;
    private Rigidbody rb;
    public float rotateSpeed = 0.025f;

    private bool targetIsInFront = false;
    private float angleRange = 30f;


    // Laser Gun Variabls
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 1f)]
    [SerializeField] private float fireRate = 0.5f;
    private float fireTimer;
    private bool targetIsInFiringRange = false;
    private float firingRange = 10f;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!target)
        {
            GetTarget();
        }
        else
        {
            RotateTowardsTarget();
        }

        if (target && target.CompareTag("Player") && targetIsInFront && targetIsInFiringRange)
        {
            // if the target is our player and they are in firing range, then fire lasers at the allowed firing rate
            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer = fireRate;
            }
            else
            {
                fireTimer -= Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        if (targetIsInFront)
        {
            // only turn on thrusters if our intended target is infront of us
            rb.AddForce(thrust * -transform.up);
        }
    }

    private void Shoot()
    {
        // shoot the laser and give it the damage amount of our entity
        GameObject laser = Instantiate(laserPrefab, firingPoint.position, firingPoint.rotation);
        laser.GetComponent<Laser>().damage = gameObject.GetComponent<EntityStats>().damage;
    }

    private void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void RotateTowardsTarget()
    {
        Vector3 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + 90f;
        float angle2 = Vector3.Angle(transform.up, targetDirection);

        targetIsInFront = angle2 >= 180 - angleRange || angle2 <= 180 + angleRange;
        targetIsInFiringRange = targetDirection.magnitude <= firingRange;

        Quaternion newRotation = Quaternion.Euler(0, 0, angle);
        // slowly interpolate to that rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotation, rotateSpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        // if they crash the enemy destroys the player
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            target = null;
        }
    }
}

