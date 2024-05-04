using System.Collections.Generic;
using UnityEngine;

public class Asteroid : EntityStats
{
    // Pickups
    [SerializeField] private GameObject DeathAnimation;
    public List<GameObject> PickupPrefabs = new();
    public List<GameObject> DebrisPrefabs = new();

    // Collision
    public float collisionDebounce = 1.5f; // seconds to not inflict damage
    private float collisionTimer;

    // Stats
    private readonly float minHealth = 10f;
    private readonly float maxHealthLimit = 40f;
    private readonly float minDamage = 5f;
    private readonly float maxDamage = 10f;
    public Vector3 initialForce;


    // Start is called before the first frame update
    void Start()
    {
        maxHealth = Random.Range(minHealth, maxHealthLimit);
        currentHealth = maxHealth;
        damage = Map(maxHealth, minHealth, maxHealthLimit, minDamage, maxDamage);
        armor = 0f;

        // Make the asteroid have some initial spinning inertia
        gameObject.GetComponent<Rigidbody>().AddTorque(transform.up * Random.Range(-4f, 4f));
        gameObject.GetComponent<Rigidbody>().AddTorque(transform.right * Random.Range(-4f, 4f));
        if (initialForce != null) gameObject.GetComponent<Rigidbody>().AddForce(initialForce);

    }


    private void OnCollisionEnter(Collision other)
    {
        if (isDead) return;

        Debug.Log("Asteroid Collision: " + other.gameObject.name + " " + other.gameObject.tag);
        // If player collides with asteroid they take damage, then give them a few seconds where we don't reapply damage
        if (other.gameObject.CompareTag("Player"))
        {
            if (collisionTimer <= 0f)
            {
                other.gameObject.TryGetComponent<EntityStats>(out var stats); // if game object has entity stats then take damage

                if (stats) stats.TakeDamage(damage);

                // Reset collision debouncing
                collisionTimer = collisionDebounce;
            }
            else
            {
                collisionTimer -= Time.deltaTime;
            }
        }
    }

    public override void Die()
    {
        // Prevent multiple deaths getting called
        if (isDead) return;
        isDead = true;

        // Play explosion animation/sound
        dieSoundSource.Play();
        GameObject anim = Instantiate(DeathAnimation, transform.position, DeathAnimation.transform.rotation);
        Destroy(anim, 2f); // Destory object after short time

        bool isDebrisSize = damage < minDamage || maxHealth < minHealth;
        // Don't leave debris if asteroid is puny or debris
        if (!isDebrisSize) LeaveDebris();

        LeavePickup();

        // Give the player XP
        PlayerStatsManager.Instance.points += Mathf.RoundToInt(Map(maxHealthLimit, 10f, 40f, 1f, 3f));

        // Destory object
        GetComponent<MeshRenderer>().enabled = false; // hide asteroid
        Destroy(gameObject, dieSoundSource.clip.length + 0.1f);
        isDead = true;
    }

    private void LeavePickup()
    {
        // 5% chance of leaving something if is debris sized
        // 50% chance that the asteroid has a pickup
        bool isDebrisSize = damage < minDamage || maxHealth < minHealth;
        float chanceOfPickup = isDebrisSize ? 5f : 100f;
        if (Random.Range(0, 100) < chanceOfPickup)    // todo change back to 50%
        {
            GameObject selectedPickup = PickupPrefabs[Random.Range(0, PickupPrefabs.Count)];
            Debug.Log("LeavePickup: " + selectedPickup.name);
            GameObject pickupObject = Instantiate(selectedPickup, transform.position, selectedPickup.transform.rotation);
            pickupObject.GetComponent<Pickup>().pickupValue = GetPickupValue(selectedPickup);
        }
    }

    private void LeaveDebris()
    {
        // Instantiate debris, which is a smaller weaker asteroid
        int debrisQty = Random.Range(2, 5);
        for (int i = 0; i < debrisQty; i++)
        {
            // Get prefab and instantiate
            GameObject debrisPrefab = DebrisPrefabs[Random.Range(0, DebrisPrefabs.Count)];
            GameObject debrisObject = Instantiate(debrisPrefab, transform.position, debrisPrefab.transform.rotation);

            // Create a direction vector to add force to debris explosion, no force in the Z direction
            Vector3 direction = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            debrisObject.GetComponent<Rigidbody>().AddForce(direction);
            Asteroid asteroid = GetComponentInChildren<Asteroid>();

            // Make the debris weak asteroids;
            if (asteroid)
            {
                asteroid.maxHealth = 5f;
                asteroid.currentHealth = asteroid.maxHealth;
                asteroid.damage = 1f;
                asteroid.armor = 0f;
            }
        }
    }

    private float GetPickupValue(GameObject selectedPickup)
    {
        // Give more points at higher xp levels
        int xpMultiplier = (int)Mathf.Max(1, PlayerStatsManager.Instance.points / 120);
        float pickupValue = 5 * xpMultiplier;
        if (selectedPickup.CompareTag("Fuel"))
        {
            pickupValue = Random.Range(2 * xpMultiplier, 7 * xpMultiplier);
        }
        else if (selectedPickup.CompareTag("Health"))
        {
            pickupValue = Random.Range(2 * xpMultiplier, 5 * xpMultiplier);
        }
        else if (selectedPickup.CompareTag("Parts"))
        {
            pickupValue = Random.Range(1 * xpMultiplier, 2 * xpMultiplier);
        }
        return pickupValue;
    }

    private float Map(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        // normalize the value within the input range
        float normalizedValue = (value - inputMin) / (inputMax - inputMin);

        // scale the normalized value to the output range
        return outputMin + (normalizedValue * (outputMax - outputMin));
    }


}
