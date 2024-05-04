using UnityEngine;

public class Laser : MonoBehaviour
{

    [Range(1, 10)]
    [SerializeField] private float speed = 10f;

    [Range(1, 10)]
    public float range = 3f;    // Lifetime of laser in seconds
    public float damage = 10f;

    // Prevent lasers from doing damage to the shooter
    private float collisionTimer = 0;
    private readonly float collisionBuffer = 0.01f;
    private string shooterTag;

    [SerializeField] private GameObject explosionEffect;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, range);
    }

    // Using fixed update since we are updating physics based properties
    void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
        collisionTimer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {

        // Let laser get past the shooter's own collider
        if (collisionTimer > collisionBuffer)
        {
            // Turn off friendly fire
            if (other.gameObject.CompareTag(shooterTag)) return;

            // If game object has entity stats then take damage
            other.gameObject.TryGetComponent<EntityStats>(out var stats);
            if (stats) stats.TakeDamage(damage);

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
        Destroy(explosion, 2f); // 2 seconds is the durration of the explosion effect, but not sure how to access that currently. // TODO
    }
}
