using UnityEngine;

public class Laser : MonoBehaviour
{

    [Range(1, 10)]
    [SerializeField] private float speed = 10f;

    [Range(1, 10)]
    public float range = 3f;

    public float damage = 10f;

    // Prevent lasers from doing damage to the shooter
    private float collisionTimer = 0;
    private readonly float collisionBuffer = 0.05f;
    private string shooterTag;

    [SerializeField] private GameObject explosionEffect;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, range);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
        collisionTimer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<EntityStats>(out var stats); // if game object has entity stats then take damage

        if (collisionTimer > collisionBuffer) // Let laser get past the collider
        {
            if (other.gameObject.CompareTag(shooterTag))
            {
                // turn off friendly fire so enemies can't destroy each other
                // do nothing
            }
            else if (stats)
            {
                stats.TakeDamage(damage);
            }
            Die();
        }
    }

    public void SetShooterTag(string tag)
    {
        shooterTag = tag;
    }

    private void Die()
    {
        // Destroy laser
        Destroy(gameObject);

        // Bullet Collision animation
        GameObject explosion = Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(explosion, 2f);
    }
}
