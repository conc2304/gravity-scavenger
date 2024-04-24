using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{

    public Transform target;
    public float thrust = 3f;
    private Rigidbody rb;
    public float rotateSpeed = 0.05f;

    // [SerializeField] private GameObject explosionEffect;

    private bool targetIsInFront = false;
    private readonly float angleRange = 30f; // used to calculate if target is in front


    // Laser Gun Variabls
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 2f)]
    [SerializeField] private float fireRate = 2f;
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
        laser.GetComponent<Laser>().SetShooterTag(gameObject.tag);

        gameObject.TryGetComponent<EntityStats>(out var stats); // if game object has entity stats then take damage
        if (stats)
        {
            Laser laserStats = laser.GetComponent<Laser>();

            laserStats.damage = stats.damage;
            laserStats.range = stats.firingRange;
        }
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
            other.gameObject.GetComponent<PlayerStats>().Die();
            target = null;

            // scavenge the players 

            // restart scene after a few seconds
            // todo add delay
            SceneManager.LoadScene("Play");
        }
    }
}

