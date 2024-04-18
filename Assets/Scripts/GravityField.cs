using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    // A list of objects that we explicitly want to attract
    public List<GameObject> attractees = new List<GameObject>();
    private Rigidbody rb;

    [SerializeField]
    private float maxDistance = 20f;

    void Start()
    {
        // All gravity fields should at a minumun attract the player
        GameObject player = GameObject.FindWithTag("Player");
        rb = this.GetComponent<Rigidbody>();

        // Check if a player GameObject
        if (player != null)
        {
            AddAttractee(player);
        }
        else
        {
            // If no GameObject with the specified tag was found
            Debug.LogWarning("No GameObject with tag 'Player' found.");
        }
    }

    private void FixedUpdate()
    {
        // Loop through all attractees in the list
        foreach (GameObject attractee in attractees)
        {
            Attract(attractee);
        }
    }

    private void Attract(GameObject attractee)
    {
        // Calculate distance to this attractee
        Vector3 force;
        Rigidbody rbToAttract = attractee.GetComponent<Rigidbody>();
        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        if (distance == 0f) return; // Don't divide by zero

        // don't apply force if it is too far
        if (distance > maxDistance)
        {
            force = new Vector3(0, 0, 0);
        }
        else
        {
            float forceMagnitude = rb.mass * rbToAttract.mass / Mathf.Pow(distance, 2);
            force = direction.normalized * forceMagnitude;
        }

        rbToAttract.AddForce(force);
    }

    // Add a new attractee to the list
    public void AddAttractee(GameObject newAttractee)
    {
        attractees.Add(newAttractee);
    }

    // Remove an attractee from the list
    public void RemoveAttractee(GameObject attracteeToRemove)
    {
        attractees.Remove(attracteeToRemove);
    }
}