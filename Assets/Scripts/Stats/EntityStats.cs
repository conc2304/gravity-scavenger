using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; } // any script can get current health, but only set it here

    public int armor = 10;
    public int damage = 10;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        print("Take Damage: " + gameObject.tag + " - " + damage);
        // let armor and sheild take some of the damage
        // damage -= shield.GetValue();
        damage -= armor;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        currentHealth -= damage;
        print(currentHealth + " - " + damage);

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
