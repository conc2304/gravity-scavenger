using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// Derive Player Stats from entity Stats
public class PlayerStats : EntityStats
{
    // Player stats
    private float maxFuel;
    private float currentFuel;
    private float fuelRate;
    private float parts;
    private float points;
    private float lives;

    // UI References
    [SerializeField] private StatusBar healthBar;
    [SerializeField] private StatusBar fuelBar;
    [SerializeField] private Text partsText;
    [SerializeField] private Text pointsText;

    // Animation Reference
    [SerializeField] private GameObject DeathAnimation;


    private void Start()
    {
        // Initialize Stats from the Persistent Stats Manager
        // Upgradeable stats have currentValue
        currentFuel = PlayerStatsManager.Instance.maxFuel.currentValue;
        currentHealth = PlayerStatsManager.Instance.maxHealth.currentValue;
        maxFuel = PlayerStatsManager.Instance.maxFuel.currentValue;
        maxHealth = PlayerStatsManager.Instance.maxHealth.currentValue;
        damage = PlayerStatsManager.Instance.damage.currentValue;
        armor = PlayerStatsManager.Instance.armor.currentValue;
        thrust = PlayerStatsManager.Instance.thrust.currentValue;

        // Non-upgradable stats
        fuelRate = PlayerStatsManager.Instance.fuelRate;
        parts = PlayerStatsManager.Instance.parts;
        points = PlayerStatsManager.Instance.points;

        // Initialize the UI
        healthBar.SetSliderMax(maxHealth);
        fuelBar.SetSliderMax(maxFuel);
    }

    // Class Methods

    public void DepleteFuel()
    {
        currentFuel -= fuelRate;
        UpdateUI();
        PlayerStatsManager.Instance.currentFuel = currentFuel;

        if (currentFuel <= 0)
        {
            Die();
        }
    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        UpdateUI();
        PlayerStatsManager.Instance.currentFuel = currentFuel;
    }

    public void AddHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
        PlayerStatsManager.Instance.currentHealth = currentHealth;
    }

    public void AddParts(float amount)
    {
        parts += amount;
        UpdateUI();
        PlayerStatsManager.Instance.parts = parts;
    }

    public void StealParts(float amount)
    {
        parts -= amount;
        parts = Mathf.Clamp(parts, 0, Mathf.Infinity); // Can't steal what they don't have
        UpdateUI();
        PlayerStatsManager.Instance.parts = parts;
    }

    public void AddPoints(float amount)
    {
        points += amount;
        UpdateUI();
        PlayerStatsManager.Instance.points = points;
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
            pointsText.text = Mathf.Round(points).ToString() + " XP";
    }

    public new void TakeDamage(float damage)
    {
        Debug.Log("Take Damage PLayer Stats");
        GetComponent<EnemyStats>().TakeDamage(damage);
        PlayerStatsManager.Instance.currentHealth = currentHealth;
    }

    public override void Die()
    {
        // Play die animation
        GameObject anim = Instantiate(DeathAnimation, transform.position, DeathAnimation.transform.rotation);
        Destroy(anim, 1.75f); // destory animation of short time

        // Lose a life
        lives -= 1;
        if (lives <= 0)
        {
            StartCoroutine(GameOver());
        }

        // Lose some parts if we have any
        StealParts(Random.Range(1, 3));
    }

    IEnumerator GameOver()
    {
        // Pause 2 seconds and then load game over screen
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Game Over");
    }
}
