using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public float armor = 10f;
    public float damage = 10f;

    public float firingRange = 3f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        // let armor and sheild take some of the damage
        // damage -= shield.GetValue();
        damage -= armor;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        // Update health bar
        if (this is PlayerStats stats)
        {
            stats.UpdateHealthBar();
        }
    }

    public virtual void Die()
    {
        // Die in some way
        // this method is meant to be overwritten per instance
    }

}
