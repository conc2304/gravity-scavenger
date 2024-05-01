using System.Collections.Generic;
using UnityEngine;

// Derive Enemy Stats from entity Stats
public class EnemyStats : EntityStats
{
    private GameObject Player;
    [SerializeField] private GameObject DeathAnimation;

    // Pickups
    public List<GameObject> PickupPrefabs = new();
    private readonly Dictionary<int, int> pickUpProbability = new()
        {
            // Index matches the order in which the prefabs where added in the inspector
            { 60, 2 },  // Parts[2] (60% probability)
            { 85, 0 },  // Fuel[0] (25% probability, added to previous percentage)
            { 100, 1 }  // Health[1] (15% probability, added to previous percentage)
        };


    // Unity MonoBehaviour Methods

    private void Start()
    {
        if (!Player) Player = GameObject.FindGameObjectWithTag("Player");

        // TODO make these change based on player xp
        maxHealth = 30f;
        currentHealth = maxHealth;
        firingRange = 0.3f;
        fireRate = 5f;
        damage = 7f;
    }

    // Class Methods


    // Based on a percent probability, select an index that maps to the probability
    public int SelectItemIndex(int percent)
    {
        // Iterate through the probability mappings
        foreach (var kvp in pickUpProbability)
        {
            // Check if the given percent falls within the current range
            if (percent <= kvp.Key)
            {
                // Return the  index
                return kvp.Value;
            }
        }

        // If percent is greater than the highest defined range, return the last item
        return pickUpProbability[pickUpProbability.Count];
    }

    public override void Die()
    {
        // Prevent multiple deaths getting called
        if (isDead) return;
        isDead = true;

        // Play die animation
        dieSoundSource.Play();
        GameObject anim = Instantiate(DeathAnimation, transform.position, DeathAnimation.transform.rotation);
        Destroy(anim, 1.75f); // destory animation of short time

        // Leave pickups/parts for player to scavenge
        int itemIndex = SelectItemIndex(Random.Range(0, 100));
        GameObject selectedPickup = PickupPrefabs[itemIndex];
        GameObject pickupObject = Instantiate(selectedPickup, transform.position, selectedPickup.transform.rotation);

        // Set different pickup values for different items
        float pickupValue = 5;
        if (selectedPickup.CompareTag("Fuel"))
        {
            pickupValue = Random.Range(10, 15); // TODO make points dynamic basic on level difficulty
        }
        else if (selectedPickup.CompareTag("Health"))
        {
            pickupValue = Random.Range(10, 20); // TODO make points dynamic basic on level difficulty
        }
        else if (selectedPickup.CompareTag("Parts"))
        {
            pickupValue = Random.Range(1, 5); // TODO make points dynamic basic on level difficulty
        }
        pickupObject.GetComponent<PowerUp>().pickupValue = pickupValue;

        // Give the player game points for killing an enemy
        Player.GetComponent<PlayerStats>().AddPoints(10);   // TODO make points dynamic basic on level difficulty

        // Kill Enemy Entity
        // GetComponent<Renderer>().enabled = false;
        Destroy(gameObject, dieSoundSource.clip.length + 0.3f);
    }

}
