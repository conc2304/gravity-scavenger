using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other, true);
        }
        else if (other.CompareTag("Enemy"))
        {
            Pickup(other, false);
        }
    }

    private void Pickup(Collider player, bool applyPowerUp)
    {
        Debug.Log("Power up Picked up");

        // Spawn pickup effect
        GameObject pickupEffectInstance = Instantiate(pickupEffect, transform.position, transform.rotation);
        Destroy(pickupEffectInstance, 2f); // destroy it after its done playing


        // Apply power up effect
        if (applyPowerUp)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            // Todo make this dynamic based on the type of pickup
            playerStats.AddFuel(10f);
        }
        // Remove power up object
        Destroy(gameObject);
    }
}
