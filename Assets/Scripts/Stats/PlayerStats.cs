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
    public float currentFuel;
    public float parts = 0f;

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
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        UpdateFuelBar();
    }

    public void AddHealth(float amount)
    {
        Debug.Log("Add Health: " + amount);
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    public void AddParts(float amount)
    {
        parts += amount;
    }

    public void StealParts(float amount)
    {
        parts -= amount;
    }
}
