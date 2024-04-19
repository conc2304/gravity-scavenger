using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Derive Player Stats from entity Stats
public class EnemyStats : EntityStats
{


    // todo add health bar
    // public StatusBar healthBar;


    // Start is called before the first frame update
    // private void Start()
    // {
    //     // healthBar.SetSliderMax(maxHealth);
    // }



    public void UpdateHealthBar()
    {
        // healthBar.SetSlider(currentHealth);
    }



    public override void Die()
    {

        // Kill Enemy
        Destroy(gameObject);

        // play die animation

        // give the player points
    }
}
