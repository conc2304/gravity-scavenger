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

    private int xpMultiplier = 1; // Used to dynamically scale the values of enemy stats and points given

    // Unity MonoBehaviour Methods

    private void Start()
    {
        if (!Player) Player = GameObject.FindGameObjectWithTag("Player");

        // for every 120xp over 250xp increment the xpMyultiplier by 1
        float playerXP = PlayerStatsManager.Instance.points;
        int xpMultiplier = (int)(Mathf.Max(1f, playerXP - 250) / 120);

        // Dynamic initial stats based on XP
        maxHealth = Random.Range(20f, 20f * xpMultiplier);
        currentHealth = maxHealth;
        firingRange = 0.3f * (xpMultiplier > 1 ? xpMultiplier / 1.2f : 1f);
        fireRate = 5f / xpMultiplier;
        damage = 7f * xpMultiplier;
    }

    // Class Methods


    // Based on a percent probability, select an index that maps to the probability
    public int SelectItemIndex(int percent)
    {
        // Iterate through the probability mappings
        foreach (KeyValuePair<int, int> kvp in pickUpProbability)
        {
            int probability = kvp.Key;
            int prefabIndex = kvp.Value;

            // Check if the given percent falls within the current range
            if (percent <= probability)
            {
                return prefabIndex;
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
        pickupObject.GetComponent<Pickup>().pickupValue = GetPickupValue(selectedPickup);

        // Give the player game points for killing an enemy
        Player.GetComponent<PlayerStats>().AddPoints(10 * xpMultiplier);

        // Kill Enemy Entity
        GetComponent<MeshRenderer>().enabled = false; // hide the ship 
        Destroy(gameObject, dieSoundSource.clip.length + 0.3f);
    }


    // Give different items different pickup values
    private float GetPickupValue(GameObject selectedPickup)
    {

        float pickupValue = 5 * xpMultiplier;

        if (selectedPickup.CompareTag("Fuel"))
        {
            pickupValue = Random.Range(10 * xpMultiplier, 15 * xpMultiplier);
        }
        else if (selectedPickup.CompareTag("Health"))
        {
            pickupValue = Random.Range(10 * xpMultiplier, 20 * xpMultiplier);
        }
        else if (selectedPickup.CompareTag("Parts"))
        {
            pickupValue = Random.Range(3 * xpMultiplier, 7 * xpMultiplier);
        }
        return pickupValue;
    }

}
