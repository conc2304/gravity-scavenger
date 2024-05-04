using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

// Manages the UI Game Play Stat
public class GamePlayStatsBar : MonoBehaviour
{

    [SerializeField] private UIDocument gamePlayStatsDocument; // UI Document to update
    [SerializeField] private VisualTreeAsset lifeTemplate; // UI component/template for the lives (reusing the upgradeSlot from player upgrade)
    private VisualElement gamePlayStatsRoot; // Root element to base queries off of

    // UI components to bind data to
    private ProgressBar healthBar;
    private ProgressBar fuelBar;
    private TextElement partsValue;
    private TextElement pointsValue;
    private VisualElement healthProgressWrapper;
    private VisualElement fuelProgressWrapper;
    private VisualElement livesStatusContainer;

    // Base and Max width to align with upgrades to fuel and health
    private readonly float progressBaseMaxWidth = 50f;
    private readonly float progressFinalMaxWidth = 100f;

    private bool updatedOnce = false;

    private void Start()
    {
        Debug.Log("Enable GamePlayStatsBar");

        // Get the UI elements from the root document
        gamePlayStatsRoot = gamePlayStatsDocument.rootVisualElement;
        healthBar = gamePlayStatsRoot.Q<ProgressBar>("healthProgressBar");
        fuelBar = gamePlayStatsRoot.Q<ProgressBar>("fuelProgressBar");
        partsValue = gamePlayStatsRoot.Q<TextElement>("partsValue");
        pointsValue = gamePlayStatsRoot.Q<TextElement>("pointsValue");
        healthProgressWrapper = gamePlayStatsRoot.Q<VisualElement>("healthProgressWrapper");
        fuelProgressWrapper = gamePlayStatsRoot.Q<VisualElement>("fuelProgressWrapper");
        livesStatusContainer = gamePlayStatsRoot.Q<VisualElement>("livesStatus");

        if (PlayerStatsManager.Instance != null) UpdateUI();
    }

    private void Update()
    {
        // Update the UI immediately once Stats Manager is available
        if (PlayerStatsManager.Instance != null && !updatedOnce)
        {
            UpdateUI();
            updatedOnce = true;
        }

    }

    public void UpdateUI()
    {
        // Update Progress Bar Size
        // Make the progress bar grow in width based on max fuel/health to show upgrades in max fuel/health
        // For each upgrade used, increment the maxWidth of the progress bar container so that it gets longer
        // if health or fuel are below 15% then add the danger class if not remove it

        // Update the Progress Bar values
        if (healthBar != null)
        {
            healthBar.highValue = PlayerStatsManager.Instance.maxHealth.currentValue;
            healthBar.value = PlayerStatsManager.Instance.currentHealth;

            float healthIncAmount = (progressFinalMaxWidth - progressBaseMaxWidth) / PlayerStatsManager.Instance.maxHealth.maxUpgrades;
            healthProgressWrapper.style.maxWidth = Length.Percent(progressBaseMaxWidth + (healthIncAmount * PlayerStatsManager.Instance.maxHealth.upgradesUsed));
            if (PlayerStatsManager.Instance.currentHealth / PlayerStatsManager.Instance.maxHealth.currentValue < 0.15)
            {
                healthProgressWrapper.Q<VisualElement>(className: "unity-progress-bar__background").AddToClassList("danger");
            }
            else
            {
                healthProgressWrapper.Q<VisualElement>(className: "unity-progress-bar__background").RemoveFromClassList("danger");
            }
        }

        if (fuelBar != null)
        {

            fuelBar.highValue = PlayerStatsManager.Instance.maxFuel.currentValue;
            fuelBar.value = PlayerStatsManager.Instance.currentFuel;

            float fuelIncAmount = (progressFinalMaxWidth - progressBaseMaxWidth) / PlayerStatsManager.Instance.maxFuel.maxUpgrades;
            fuelProgressWrapper.style.maxWidth = Length.Percent(progressBaseMaxWidth + (fuelIncAmount * PlayerStatsManager.Instance.maxFuel.upgradesUsed));
            if (PlayerStatsManager.Instance.currentFuel / PlayerStatsManager.Instance.maxFuel.currentValue < 0.20)
            {
                fuelProgressWrapper.Q<VisualElement>(className: "unity-progress-bar__background").AddToClassList("danger");
            }
            else
            {
                fuelProgressWrapper.Q<VisualElement>(className: "unity-progress-bar__background").RemoveFromClassList("danger");
            }
        }

        if (partsValue != null)
        {
            // Update Parts Text
            partsValue.text = PlayerStatsManager.Instance.parts.ToString();
        }

        if (pointsValue != null)
        {
            // Update Points Text
            pointsValue.text = PlayerStatsManager.Instance.points.ToString();
        }


        // Update lives values
        List<VisualElement> upgradeSlots = gamePlayStatsRoot.Query<VisualElement>(className: "upgradeSlot").ToList();

        // Instantiate if not there
        if (upgradeSlots == null || upgradeSlots.Count == 0)
        {
            // Add lives to the lives status container
            for (int i = 0; i < PlayerStatsManager.Instance.maxLives; i++)
            {
                VisualElement upgradeSlot = lifeTemplate.CloneTree();

                upgradeSlot.style.flexGrow = 1; // Hack to fix template containeer not respecting child styles

                // Set the lives as "unused" by reusing the "slotUsed" class from upgrade system
                if (PlayerStatsManager.Instance.lives > i)
                {
                    upgradeSlot.AddToClassList("slotUsed");
                }

                // Add component to parent container
                livesStatusContainer.Add(upgradeSlot);
            }
        }


        // Update lives values
        upgradeSlots = gamePlayStatsRoot.Query<VisualElement>(className: "upgradeSlot").ToList();
        int j = 0;
        foreach (VisualElement slot in upgradeSlots)
        {

            // If upgrade has ben used mark it by add the slotUsed class, otherwise remove class
            if (PlayerStatsManager.Instance.lives > j)
            {
                slot.AddToClassList("slotUsed");
            }
            else
            {
                slot.RemoveFromClassList("slotUsed");
            }

            j++;
        }
    }
}
