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
    private readonly float minHealth = 10f;
    private readonly float maxHealthLimit = 40f;
    private readonly float minDamage = 5f;
    private readonly float maxDamage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = Random.Range(minHealth, maxHealthLimit);
        currentHealth = maxHealth;
        damage = Map(maxHealth, minHealth, maxHealthLimit, minDamage, maxDamage);
        armor = 0f;

        // Make the asteroid have some initial spinning inertia
        gameObject.GetComponent<Rigidbody>().AddTorque(transform.up * Random.Range(-3f, 3f));
        gameObject.GetComponent<Rigidbody>().AddTorque(transform.right * Random.Range(-3f, 3f));
    }


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Asteroid Collision");
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

        // Instantiate smaller debris, if not already debris
        LeaveDebris();

        LeavePickup();

        // Give the player XP
        PlayerStatsManager.Instance.points += Mathf.RoundToInt(Map(maxHealthLimit, 10f, 40f, 1f, 3f));

        // Destory object
        GetComponent<MeshRenderer>().enabled = false; // hide asteroid
        Destroy(gameObject, dieSoundSource.clip.length + 0.1f);
    }

    private void LeavePickup()
    {
        // Don't leave pickup if asteroid is puny or debris
        if (damage < minDamage || maxHealth < minHealth) return;

        // 50% chance that the asteroid has a pickup
        if (Random.Range(0, 100) < 50f)
        {
            GameObject selectedPickup = PickupPrefabs[Random.Range(0, PickupPrefabs.Count - 1)];
            GameObject pickupObject = Instantiate(selectedPickup, transform.position, selectedPickup.transform.rotation);
            pickupObject.GetComponent<PowerUp>().pickupValue = GetPickupValue(selectedPickup);
        }
    }

    private void LeaveDebris()
    {
        // Debris sized asteroids don't leave more debris
        if (damage < minDamage || maxHealth < minHealth) return;

        int debrisQty = Random.Range(2, 4);
        for (int i = 0; i < debrisQty; i++)
        {

            Vector3 direction = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            GameObject selectedPickup = DebrisPrefabs[Random.Range(0, DebrisPrefabs.Count - 1)];
            GameObject debrisObject = Instantiate(selectedPickup, transform.position, selectedPickup.transform.rotation);
            debrisObject.GetComponent<Rigidbody>().AddForce(direction);
            Asteroid asteroid = GetComponentInChildren<Asteroid>();

            // Make the debris weaker;
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
        float pickupValue = 5;
        if (selectedPickup.CompareTag("Fuel"))
        {
            pickupValue = Random.Range(2, 7); // TODO make points dynamic basic on level difficulty
        }
        else if (selectedPickup.CompareTag("Health"))
        {
            pickupValue = Random.Range(2, 5); // TODO make points dynamic basic on level difficulty
        }
        else if (selectedPickup.CompareTag("Parts"))
        {
            pickupValue = Random.Range(1, 2); // TODO make points dynamic basic on level difficulty
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
