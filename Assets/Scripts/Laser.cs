using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [Range(1, 10)]
    [SerializeField] private float speed = 10f;

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f;

    public int damage = 10;

    // prevent lasers from doing damage to the shooter
    private float collisionTimer = 0;
    private float collisionBuffer = 0.05f;


    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.up * speed;

        collisionTimer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {

        if (collisionTimer > collisionBuffer)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<EntityStats>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
