using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

// Persistence Layer of Player Stats
public class PlayerStatsManager : MonoBehaviour
{

    public static PlayerStatsManager Instance;

    public float lives = 3f;
    // public float maxFuel = 100f;
    public float currentFuel;
    public float fuelRate = 0.01f;
    public float parts = 15f;
    public float points = 0f;
    // public float maxHealth = 100f;
    public float currentHealth;
    // public float armor = 10f;
    // public float damage = 10f;
    // public float fireRate = 1f;
    // public float firingRange = 3f;
    // public float thrust = 150f;

    // upgradables
    public static readonly int MaxUpgrades = 4;
    public UpgradableStat maxHealth = new("Max Health", 100f, 200f, MaxUpgrades, 5f);
    public UpgradableStat maxFuel = new("Max Fuel", 100f, 200f, MaxUpgrades, 8f);
    public UpgradableStat armor = new("Armour", 5f, 40f, MaxUpgrades, 6f);
    public UpgradableStat damage = new("Damage", 10f, 50f, MaxUpgrades, 10f);
    public UpgradableStat fireRange = new("Fire Range", 3f, 5f, MaxUpgrades, 4f);
    public UpgradableStat fireRate = new("Fire Rate", 1f, 0.2f, MaxUpgrades, 5f);
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

        // Initialize Upgradeable Stats
    }

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