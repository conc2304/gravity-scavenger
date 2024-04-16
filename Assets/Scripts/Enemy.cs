using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Transform target;
    public float thrust = 3f;
    private Rigidbody rb;
    public float rotateSpeed = 0.025f;

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
    }

    private void FixedUpdate()
    {
        rb.AddForce(thrust * -transform.up);
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

        Quaternion newRotation = Quaternion.Euler(0, 0, angle);
        // slowly interpolate to that rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotation, rotateSpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            target = null;
        }
        else if (other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
