using UnityEngine;

public class AddAttractee : MonoBehaviour
{
    public GameObject attracteeToAdd; // Reference to the attractee to add
    public GravityField[] gravityFields; // Reference to the GravityField script

    void Start()
    {
        // Add Attractee to all GravityFields so that it is affected by all gravity fields
        // Get a reference to the GravityField script
        gravityFields = FindObjectsOfType<GravityField>();
        foreach (GravityField gravityField in gravityFields)
        {
            // Check if the gravityField reference is not null
            if (gravityField != null && gravityField.GetComponent<Rigidbody>() != null)
            {
                // Add the attractee to the GravityField's list of attractees
                gravityField.AddAttractee(attracteeToAdd);
            }
            else
            {
                Debug.LogError("Invalid GravityFild object");
            }
        }

    }
}
