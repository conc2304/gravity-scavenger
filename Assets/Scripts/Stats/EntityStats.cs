using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; } // any script can get current health, but only set it here

    public Stat health;
    public Stat damage;
    public Stat thrust;
    public Stat armor;
    public Stat fireRate;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // let armor and sheild take some of the damage
        // damage -= shield.GetValue();
        damage -= armor.GetValue();
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
        // this method is meant to be overwritten per instance
    }

}
