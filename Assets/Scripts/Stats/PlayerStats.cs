using UnityEngine;
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

        if (currentFuel <= 0)
        {
            Die();
        }
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

    public new void TakeDamage(float damage)
    {
        Debug.Log("Take Damage Player Stats");
        GetComponent<EntityStats>().TakeDamage(damage); // Use parent method, and update stats/ui
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
        // So are hiding the childe
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
            StartCoroutine(LoadSceneAfter("Game Over", delay));
        }
        else
        {
            StartCoroutine(LoadSceneAfter("Play", delay));
        }
    }


    // Reset physics for a single Rigidbody
    public void ResetPhysics(Rigidbody rb)
    {
        if (rb != null)
        {
            rb.ResetCenterOfMass();
            rb.ResetInertiaTensor();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }



    IEnumerator LoadSceneAfter(string scene, float delay)
    {
        // Pause x seconds and then load scene
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }
}
