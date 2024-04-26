using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnOnDistance : MonoBehaviour
{
    private Transform PlayerTransform;
    public float DespawnDistance = 200f;

    private void Awake()
    {
        if (!PlayerTransform) PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerTransform) return;

        // Get the distance from player;  
        float distanceFromPlayer = Vector3.Distance(transform.position, PlayerTransform.position);
        if (distanceFromPlayer >= DespawnDistance)
        {
            // Destroy entity
            Destroy(gameObject);
        }
    }
}
