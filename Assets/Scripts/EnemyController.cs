using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Transform target;
    private Rigidbody rb;

    private float thrust;
    public float rotateSpeed = 0.1f;

    private bool targetIsInFront = false;
    private readonly float angleRange = 30f; // used to calculate if target is in front

    // Laser Gun Variabls
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private AudioSource laserSoundSource;
    private float fireTimer;
    private bool targetIsInFiringRange = false;
    private float firingRange;
    private float fireRate;
    private float damage;

    private float playerXp;
    private float aggressionLevel = 0f;
    private float anxiousLevel = 0f;
    private float maxAnxiousLevel = 10f;
    private readonly float maxAggressionLevel = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerXp = PlayerStatsManager.Instance.points;
        aggressionLevel = Random.Range(0, maxAggressionLevel);
        anxiousLevel = Random.Range(0, maxAnxiousLevel);

        // Initialize stats
        fireRate = GetComponent<EnemyStats>().fireRate;
        firingRange = GetComponent<EnemyStats>().firingRange;
        thrust = GetComponent<EnemyStats>().thrust;
        damage = GetComponent<EnemyStats>().damage;
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
        // If target is not in the non-aggression distance add force toward target
        float distance = target != null ? Vector3.Distance(transform.position, target.transform.position) : 1;

        // High aggression maps to enemies pursuing from farther away, and vice versa
        float aggressionRange = Map(aggressionLevel, 0f, maxAggressionLevel, 5f, 200f);
        bool inAggressionZone = distance < aggressionRange;

        // High anxiety map to enemies pursuing, but staying further away, and vice versa
        float anxietyZone = Map(anxiousLevel, 0f, maxAnxiousLevel, 5f, 0f);
        bool inAnxietyZone = distance < anxietyZone;

        bool isBelowMaxSpeed = rb.velocity.magnitude < 3; // Prevent ship from going too fast
        if (targetIsInFront && isBelowMaxSpeed && inAggressionZone && !inAnxietyZone)
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

        gameObject.TryGetComponent<EnemyStats>(out var stats); // if game object has entity stats then take damage
        if (stats)
        {
            Laser laserStats = laser.GetComponent<Laser>();
            laserStats.damage = damage;
            laserStats.range = firingRange;
        }

        // Play Laser audio
        laserSoundSource.Play();
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
        if (target == null) return;

        // Calculate the left and right bounds of the angle
        Vector3 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + 90f; // Account for 90 degree offset of model
        float angle2 = Vector3.Angle(transform.up, targetDirection);

        // Given an left and right bounds, determine whether the target is in front.
        targetIsInFront = angle2 >= 180 - angleRange || angle2 <= 180 + angleRange;

        targetIsInFiringRange = targetDirection.magnitude <= firingRange * 2; // let them start shooting even out of range

        // Interpolate to the new rotation
        Quaternion newRotation = Quaternion.Euler(0, 0, angle);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotation, rotateSpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (GetComponent<EnemyStats>().isDead) return;

        // If they crash the enemy destroys the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Scavenge the player's parts
            other.gameObject.GetComponent<PlayerStats>().StealParts(Random.Range(0, 3));

            // Destroy the player
            other.gameObject.GetComponent<PlayerStats>().Die();
            target = null;
        }
    }

    private float Map(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        // normalize the value within the input range
        float normalizedValue = (value - inputMin) / (inputMax - inputMin);

        // scale the normalized value to the output range
        return outputMin + (normalizedValue * (outputMax - outputMin));
    }
}

