using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Derive Player Stats from entity Stats
public class PlayerStats : EntityStats
{
    // public Stat fuel;
    // public Stat shield;
    // public Stat money;

    // only main player is concerned with shield, fuel, and money
    public float maxFuel = 100f;
    public float fuelRate = 0.01f;
    public float currentFuel { get; private set; }


    public StatusBar healthBar;
    public StatusBar fuelBar;


    // Start is called before the first frame update
    void Start()
    {
        currentFuel = maxFuel;
        healthBar.SetSliderMax(maxHealth);
        fuelBar.SetSliderMax(maxFuel);
    }

    public void UpdateHealthBar()
    {
        healthBar.SetSlider(currentHealth);
    }

    public void UpdateFuelBar()
    {
        fuelBar.SetSlider(currentFuel);
    }

    public void DepleteFuel()
    {
        currentFuel -= fuelRate;
        UpdateFuelBar();
    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;
        UpdateFuelBar();
    }
}
