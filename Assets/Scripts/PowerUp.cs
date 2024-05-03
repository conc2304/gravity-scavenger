using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float pickupValue = 10f; // value of pickup in terms of fuel/parts/health ...
    public float gamePoints = 10f; // points to award the player to accumulate points
    [SerializeField] private GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        // Ignore collision with lasers, asteroids, and dead entities
        if (other.CompareTag("Laser") || other.CompareTag("Asteroid")) return;
        if (other.TryGetComponent(out
        EntityStats stats) && stats.isDead) return;

        bool applyPowerUp = other.CompareTag("Player");
        Pickup(other, applyPowerUp);
    }

    private void Pickup(Collider player, bool applyPowerUp)
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
            playerStats.AddPoints(gamePoints);
        }
        else
        {
            // Remove power up object and prevent pickup sound from playing
            pickupEffectInstance.GetComponent<AudioSource>().enabled = false;
            Destroy(gameObject);
        }
    }


    IEnumerator DestoryAfterTime(float delay)
    {
        gameObject.transform.localScale *= 0;
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}
