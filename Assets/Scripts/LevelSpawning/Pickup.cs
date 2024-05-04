using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float pickupValue = 10f; // value of pickup in terms of fuel/parts/health ...
    public float xpValue = 10f; // points to award the player to accumulate points
    [SerializeField] private GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Power up collission PRE: " + other.gameObject.tag + " | " + other.name);
        // Ignore collision with lasers, asteroids, and dead entities
        if (other.CompareTag("Laser") || other.CompareTag("Asteroid") || other.name == "default")
        {
            Debug.Log("Collision Exit");
            return;

        }
        Debug.Log("Power up collission: " + other.tag + " | " + other.name);
        if (other.TryGetComponent(out
        EntityStats stats) && stats.isDead) return;

        bool applyEffect = other.CompareTag("Player");
        PickupItem(other, applyEffect);
    }

    private void PickupItem(Collider player, bool applyPowerUp)
    {
        // Game Object tags used to differentiate different pickup types
        string pickupType = gameObject.tag;
        Debug.Log("Power up Picked up: " + pickupType);

        // Spawn pickup effect
        GameObject pickupEffectInstance = Instantiate(pickupEffect, transform.position, transform.rotation);
        Destroy(pickupEffectInstance, 1.9f); // destroy it after its done playing

        // Apply power up effect
        if (applyPowerUp)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            // Todo make this dynamic based on the type of pickup
            if (pickupType == "Fuel") playerStats.AddFuel(pickupValue);
            if (pickupType == "Health") playerStats.AddHealth(pickupValue);
            if (pickupType == "Parts") playerStats.AddParts(pickupValue);

            // If object is destory immediately then the sound gets destroyed too
            StartCoroutine(DestoryAfterTime(2));
            gameObject.SetActive(false);
            playerStats.AddPoints(xpValue);
        }
        else
        {

            // Remove power up object and prevent pickup sound from playing
            pickupEffectInstance.GetComponent<AudioSource>().enabled = false;
            Debug.Log("Pickup Destroy Now");
            Destroy(gameObject);

        }
    }


    IEnumerator DestoryAfterTime(float delay)
    {
        Debug.Log("Pickup Destroy After");
        gameObject.transform.localScale *= 0;
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}
