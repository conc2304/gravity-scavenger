using System.Collections.Generic;
using UnityEngine;

// Persistence Layer of Player Stats
public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    // Initial Stats
    private readonly float initialLives = 3f;
    private readonly float initialFuel = 100f;
    private readonly float initialHealth = 100f;
    private readonly float initialParts = 0f;
    private readonly float initialPoints = 0f;

    // Values set in the inspector
    public float maxLives; // Todo make upgradable but ui is currently too small
    public float lives;
    public float currentFuel;
    public float fuelRate;
    public float parts;
    public float points;
    public float currentHealth;

    // Upgradable Stats
    public static readonly int MaxUpgrades = 4;
    public UpgradableStat maxHealth = new("Max Health", 100f, 200f, MaxUpgrades, 5f);
    public UpgradableStat maxFuel = new("Max Fuel", 100f, 200f, MaxUpgrades, 8f);
    public UpgradableStat armor = new("Armour", 0f, 40f, MaxUpgrades, 6f);
    public UpgradableStat damage = new("Damage", 15f, 50f, MaxUpgrades, 10f);
    public UpgradableStat fireRange = new("Fire Range", 0.40f, 1.25f, MaxUpgrades, 4f);
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

        lives = maxLives = initialLives;
        currentHealth = maxHealth.currentValue;
        currentFuel = maxFuel.currentValue;
        parts = initialParts;
        points = initialPoints;

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

    public void Reset()
    {
        lives = initialLives;
        currentFuel = initialFuel;
        currentHealth = initialHealth;
        parts = initialParts;
        points = initialPoints;

        // Reset upgradable stats to their initial values
        maxHealth.Reset();
        maxFuel.Reset();
        armor.Reset();
        damage.Reset();
        fireRange.Reset();
        fireRate.Reset();
        thrust.Reset();
    }
}