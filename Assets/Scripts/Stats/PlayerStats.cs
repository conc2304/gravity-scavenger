using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Inherit Player Stats from Entity Stats
public class PlayerStats : EntityStats
{
    // Player stats
    private float maxFuel;
    private float currentFuel;
    public float fuelRate;
    private float parts;
    private float points;
    private float lives;

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
        lives = PlayerStatsManager.Instance.lives;

    }

    // Class Methods

    public void DepleteFuel()
    {
        currentFuel -= fuelRate;
        PlayerStatsManager.Instance.currentFuel = currentFuel;
        UpdateUI();

        if (currentFuel <= 0) Die();
    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        PlayerStatsManager.Instance.currentFuel = currentFuel;
        UpdateUI();
    }

    public void AddHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        PlayerStatsManager.Instance.currentHealth = currentHealth;
        UpdateUI();
    }

    public void AddParts(float amount)
    {
        parts += amount;
        PlayerStatsManager.Instance.parts = parts;
        UpdateUI();
    }

    public void StealParts(float amount)
    {
        parts -= amount;
        parts = Mathf.Clamp(parts, 0, Mathf.Infinity); // Can't steal what they don't have
        PlayerStatsManager.Instance.parts = parts;
        UpdateUI();
    }

    public void AddPoints(float amount)
    {
        points += amount;
        PlayerStatsManager.Instance.points = points;
        UpdateUI();
    }

    public void UpdateUI()
    {
        GetComponent<GamePlayStatsBar>().UpdateUI();
    }

    public override void TakeDamage(float damage)
    {
        Debug.Log("Take Damage Player Stats");
        // Use base entity method then update stats/ui
        base.TakeDamage(damage);
        PlayerStatsManager.Instance.currentHealth = currentHealth;
        UpdateUI();
    }

    public override void Die()
    {
        // Prevent multiple collisions causing multiple deaths
        if (isDead) return;
        isDead = true;

        // Play die animation
        dieSoundSource.Play();
        GameObject anim = Instantiate(DeathAnimation, transform.position, DeathAnimation.transform.rotation);
        Destroy(anim, 2f); // Destory object after short time

        // Make the ship disappear during die animation
        // Destorying the main player game object causes errors with rigid body physics
        // So are hiding the child
        Transform childTransform = transform.Find("Ship Wrapper");
        childTransform.gameObject.SetActive(false);

        // Lose some parts if we have any
        StealParts(Random.Range(0, 3));

        // Lose a life
        float delay = Mathf.Max(dieSoundSource.clip.length, 2f) + 0.1f;
        lives -= 1;
        PlayerStatsManager.Instance.lives = lives;
        if (lives <= 0)
        {
            StartCoroutine(DoAfter(() =>
            {
                SceneManager.LoadScene("Game Over");
            }, delay));
        }
        else
        {
            StartCoroutine(DoAfter(() =>
            {
                SceneManager.LoadScene("Play");
            }, delay));
        }
    }

    IEnumerator DoAfter(System.Action callback, float delay)
    {
        // Pause x seconds and then do callback
        yield return new WaitForSeconds(delay);
        callback();
    }
}
