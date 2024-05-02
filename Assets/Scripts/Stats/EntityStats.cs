using UnityEngine;

public class EntityStats : MonoBehaviour
{

    // Base Entity Stats
    public float maxHealth = 100f;
    public float currentHealth;
    public float armor = 5f;
    public float damage = 10f;
    public float fireRate = 2.5f;    // Interval of seconds between each available shot
    public float firingRange = 3f;   // How long the shot is alive for in seconds, with speed determines range
    public float thrust;
    public bool isDead = false;

    public AudioSource dieSoundSource;

    // Unity MonoBehaviour Methods
    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Class Methods

    public virtual void TakeDamage(float damage)
    {
        // Armor reduces damage
        damage -= armor;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // Die in some way
        // This method is meant to be overwritten per instance
    }

}
