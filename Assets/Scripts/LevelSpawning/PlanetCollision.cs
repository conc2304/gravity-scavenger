using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Destroy the player
            other.gameObject.GetComponent<PlayerStats>().Die();
        }
    }
}
