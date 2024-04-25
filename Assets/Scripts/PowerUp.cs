using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject pickupEffect;
    public float pickupValue = 10f;
    public float gamePoints = 10f; // points to award the player to accumulate points

    private void OnTriggerEnter(Collider other)
    {
        bool applyPowerUp = other.CompareTag("Player");
        if (other.CompareTag("Laser")) return;
        Pickup(other, applyPowerUp);
    }

    private void Pickup(Collider player, bool applyPowerUp)
    {
        string pickupType = gameObject.tag;
        Debug.Log("Power up Picked up: " + pickupType);

        // Spawn pickup effect
        GameObject pickupEffectInstance = Instantiate(pickupEffect, transform.position, transform.rotation);
        Destroy(pickupEffectInstance, 2f); // destroy it after its done playing

        // Apply power up effect
        if (applyPowerUp)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            // Todo make this dynamic based on the type of pickup
            if (pickupType == "Fuel") playerStats.AddFuel(pickupValue);
            if (pickupType == "Health") playerStats.AddHealth(pickupValue);
            if (pickupType == "Parts") playerStats.AddParts(pickupValue);

            playerStats.AddPoints(gamePoints);
        }

        // Remove power up object
        Destroy(gameObject);
    }
}
