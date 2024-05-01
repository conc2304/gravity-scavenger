using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float pickupValue = 10f; // value of pickup in terms of fuel/parts/health ...
    public float gamePoints = 10f; // points to award the player to accumulate points
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private AudioSource pickupSoundSource;

    private void OnTriggerEnter(Collider other)
    {
        bool applyPowerUp = other.CompareTag("Player");
        if (other.CompareTag("Laser")) return;
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

            // Play Pickup Sound
            pickupSoundSource.Play();

            // If we immediately destory the object then the sound gets destroyed too
            StartCoroutine(DestoryAfterTime(pickupSoundSource.clip.length * 2));
            gameObject.SetActive(false);
            playerStats.AddPoints(gamePoints);
        }
        else
        {
            // Remove power up object
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
