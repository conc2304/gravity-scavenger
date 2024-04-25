using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Derive Player Stats from entity Stats
public class PlayerStats : EntityStats
{
    // Only main player is concerned with fuel, parts, and points
    public static float maxFuel = 100f;
    public static float fuelRate = 0.01f;
    public static float currentFuel;
    public static float parts = 0f;
    public static float points = 0f;

    // UI References
    public StatusBar healthBar;
    public StatusBar fuelBar;

    [SerializeField] private Text partsText;
    [SerializeField] private Text pointsText;

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
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Add Health: " + amount + " " + currentHealth);
        UpdateHealthBar();
    }

    public void AddParts(float amount)
    {
        parts += amount;
        UpdatePartsUI();
    }

    public void StealParts(float amount)
    {
        parts -= amount;
        parts = Mathf.Clamp(parts, 0, Mathf.Infinity); // Can't steal what they don't have
        UpdatePartsUI();
    }

    public void UpdatePartsUI()
    {
        partsText.text = "" + Mathf.Round(parts);
    }

    public void AddPoints(float amount)
    {
        points += amount;
        UpdatePointsUI();
    }

    public void UpdatePointsUI()
    {
        pointsText.text = "" + Mathf.Round(points);
    }
}
