using UnityEngine;

public class EntityStats : MonoBehaviour
{
    // Base Entity Stats
    public float currentHealth;
    public float maxHealth = 100f;
    public float armor = 10f;
    public float damage = 10f;
    public float fireRate = 1f;
    public float firingRange = 3f;

    // Unity MonoBehaviour Methods
    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Class Methods

    public void TakeDamage(float damage)
    {
        // Armor reduces damage
        damage -= armor;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        // Update Player UI
        if (this is PlayerStats stats)
        {
            stats.UpdateUI();
        }
    }

    public virtual void Die()
    {
        // Die in some way
        // This method is meant to be overwritten per instance
    }

}
