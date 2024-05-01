using System.Collections.Generic;
using UnityEngine;

// Persistence Layer of Player Stats
public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    // Basic Stats
    public float baseLives = 3f;
    public float lives;
    public float currentFuel;
    public float fuelRate = 0.05f;
    public float parts = 0f;
    public float points = 0f;
    public float currentHealth;

    // Upgradable Stats
    public static readonly int MaxUpgrades = 4;
    public UpgradableStat maxHealth = new("Max Health", 100f, 200f, MaxUpgrades, 5f);
    public UpgradableStat maxFuel = new("Max Fuel", 100f, 200f, MaxUpgrades, 8f);
    public UpgradableStat armor = new("Armour", 5f, 40f, MaxUpgrades, 6f);
    public UpgradableStat damage = new("Damage", 10f, 50f, MaxUpgrades, 10f);
    public UpgradableStat fireRange = new("Fire Range", 0.35f, 2f, MaxUpgrades, 4f);
    public UpgradableStat fireRate = new("Fire Rate", 1.75f, 0.2f, MaxUpgrades, 5f);
    public UpgradableStat thrust = new("Thrust", 150f, 250f, MaxUpgrades, 6f);

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        lives = baseLives;
        currentHealth = maxHealth.currentValue;
        currentFuel = maxFuel.currentValue;
    }

    // Initialize Upgradable Stats Getter
    public List<UpgradableStat> GetAllUpgradableStats()
    {

        List<UpgradableStat> stats = new()
        {
            maxHealth,
            maxFuel,
            armor,
            damage,
            fireRange,
            fireRate,
            thrust
        };

        return stats;
    }
}