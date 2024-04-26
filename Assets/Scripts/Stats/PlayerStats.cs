using UnityEngine;
using UnityEngine.UI;

// Derive Player Stats from entity Stats
public class PlayerStats : EntityStats
{

    // Singleton instance
    private static PlayerStats instance;
    public static PlayerStats Instance { get { return instance; } }

    // Player stats
    public float maxFuel = 100f;
    public float fuelRate = 0.01f;
    public float parts = 0f;
    public float points = 0f;

    // Current player stats
    public float currentFuel;

    // UI References
    [SerializeField] private StatusBar healthBar;
    [SerializeField] private StatusBar fuelBar;
    [SerializeField] private Text partsText;
    [SerializeField] private Text pointsText;

    // Unity MonoBehaviour Methods
    private void Awake()
    {
        // Ensure there's only one instance of PlayerStats
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep PlayerStats alive between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate PlayerStats instances
        }
    }

    private void Start()
    {
        // Initialize Stats
        currentFuel = maxFuel;
        healthBar.SetSliderMax(maxHealth);
        fuelBar.SetSliderMax(maxFuel);
    }

    // Class Methods

    public void DepleteFuel()
    {
        currentFuel -= fuelRate;
        UpdateUI();
    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        UpdateUI();
    }

    public void AddHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();

    }

    public void AddParts(float amount)
    {
        parts += amount;
        UpdateUI();
    }

    public void StealParts(float amount)
    {
        parts -= amount;
        parts = Mathf.Clamp(parts, 0, Mathf.Infinity); // Can't steal what they don't have
        UpdateUI();
    }

    public void AddPoints(float amount)
    {
        points += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (healthBar != null)
            healthBar.SetSlider(currentHealth);

        if (fuelBar != null)
            fuelBar.SetSlider(currentFuel);

        if (partsText != null)
            partsText.text = Mathf.Round(parts).ToString();

        if (pointsText != null)
            pointsText.text = Mathf.Round(points).ToString();
    }
}
