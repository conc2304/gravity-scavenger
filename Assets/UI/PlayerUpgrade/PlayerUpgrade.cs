using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class PlayerUpgrade : MonoBehaviour
{

    private UIDocument playerUpgradeDocument; // UI Document to update
    // UI Templates
    public VisualTreeAsset rowTemplate;  // UI component for upgradeable player stats
    public VisualTreeAsset upgradeSlotTemplate; // UI component for the upgrade slots

    // UI Elements
    private VisualElement parentContainer; // UI parent element for where to put player stat rows
    private TextElement availablePartsElem;
    private TextElement totalQtyElem;
    private Button upgradeBtn;

    // UI Audio
    [SerializeField] private AudioSource confirmAudio;
    [SerializeField] private AudioSource upgradeAudio;
    [SerializeField] private AudioSource downgradeAudio;
    [SerializeField] private AudioSource disabledeAudio;

    private float totalUpgradesMade = 0f;    // give players xp for upgrading their stats
    private float xpPerUpergrade = 15f;

    private float totalPartsSpent;

    readonly Dictionary<string, VisualElement> rows = new();

    private void OnEnable()
    {
        int maxUpgrades = PlayerStatsManager.MaxUpgrades;

        playerUpgradeDocument = GetComponent<UIDocument>();
        parentContainer = playerUpgradeDocument.rootVisualElement.Q("content");

        // Set the UI for available points and total used
        availablePartsElem = playerUpgradeDocument.rootVisualElement.Q<TextElement>("availableQty");
        availablePartsElem.text = PlayerStatsManager.Instance.parts.ToString();
        totalQtyElem = playerUpgradeDocument.rootVisualElement.Q<TextElement>("totalQty");
        totalQtyElem.text = totalPartsSpent.ToString();

        // Loop through each upgradable stat
        // Add a upgradeRow template for each stat
        foreach (UpgradableStat stat in PlayerStatsManager.Instance.GetAllUpgradableStats())
        {
            // Clone the template
            rows[stat.statName] = rowTemplate.CloneTree();
            VisualElement upgradeStatusContainer = rows[stat.statName].Q<VisualElement>("statusBar");

            //  Add upgrade slots to UI
            for (int i = 0; i < maxUpgrades; i++)
            {
                // Clone the template
                VisualElement upgradeSlot = upgradeSlotTemplate.CloneTree();
                upgradeSlot.style.flexGrow = 1; // Hack to fix template containeer not respecting child styles

                // If upgrade has ben used mark it by adding the slotUsed class
                if (stat.upgradesUsed > i)
                {
                    upgradeSlot.AddToClassList("slotUsed");
                }

                // Add component to parent container
                upgradeStatusContainer.Add(upgradeSlot);
            }

            // Populate the row with data from the upgradable stat
            rows[stat.statName].Q<TextElement>("statName").text = stat.statName;

            // Set click callbacks
            rows[stat.statName].Q<Button>("downgradeBtn").clicked += () => OnDowngradeStat(stat);
            rows[stat.statName].Q<Button>("upgradeBtn").clicked += () => OnUpgradeStat(stat);

            // Add the row to the parent container
            parentContainer.Add(rows[stat.statName]);

            // Populate/Update player stat based UI elements
            UpdateRowUI(stat, rows[stat.statName]);
        }

        // Set the callback for the Confirm button
        upgradeBtn = playerUpgradeDocument.rootVisualElement.Q("confirmButton") as Button;
        upgradeBtn.clicked += () => OnFinish();
    }

    private void OnUpgradeStat(UpgradableStat stat)
    {

        Debug.Log("Upgrade: " + stat.statName);
        float cost = stat.upgradeCost;

        // Only allow upgrade if player has enoug to cover cost
        if (cost <= PlayerStatsManager.Instance.parts && stat.Upgrade())
        {
            totalUpgradesMade++;

            // If we successfully upgraded the stat, then update the UI
            // Update points total
            totalPartsSpent += cost;
            PlayerStatsManager.Instance.parts -= cost;

            // We update all rows so that if upgrades are out of the price range the UI reflects that
            UpdateUI();
            upgradeAudio.Play();
        }
        else
        {
            // Play disabled audio button sound
            disabledeAudio.Play();
            Debug.Log("Too expensive: " + cost + " vs " + PlayerStatsManager.Instance.parts);
        }
    }

    private void OnDowngradeStat(UpgradableStat stat)
    {
        Debug.Log("Downgrade: " + stat.statName);

        float cost = stat.downgradeCost;
        bool success = stat.Downgrade();

        // If we successfully upgraded the stat, then update the UI
        if (success)
        {
            totalUpgradesMade--;
            // Update points total
            totalPartsSpent -= cost;
            totalQtyElem.text = totalPartsSpent.ToString();
            PlayerStatsManager.Instance.parts += cost;
            availablePartsElem.text = PlayerStatsManager.Instance.parts.ToString();

            // We update all rows so that if upgrades are out of the price range the UI reflects that
            UpdateUI();
            downgradeAudio.Play();

        }
        else
        {
            // Play disabled button sound
            disabledeAudio.Play();
        }
    }

    private void UpdateUI()
    {
        foreach (UpgradableStat uStat in PlayerStatsManager.Instance.GetAllUpgradableStats())
        {
            UpdateRowUI(uStat, rows[uStat.statName]);
        }
    }

    private void UpdateRowUI(UpgradableStat stat, VisualElement row)
    {
        Debug.Log("Update UI Row: " + stat.statName);
        // Update the "shoping cart" totals
        availablePartsElem.text = PlayerStatsManager.Instance.parts.ToString();
        totalQtyElem.text = totalPartsSpent.ToString();

        bool upgradeTooExpensive = stat.upgradeCost > PlayerStatsManager.Instance.parts;

        // Update UI if the player can't upgrade or downgrade the stat
        if (stat.isDowngradable)
        {
            row.Q<Button>("downgradeBtn").RemoveFromClassList("disabled");
            row.Q<TextElement>("downgradeCost").text = stat.downgradeCost.ToString();
        }
        else
        {
            row.Q<Button>("downgradeBtn").AddToClassList("disabled");
            row.Q<TextElement>("downgradeCost").text = "X";
        }

        if (stat.isUpgradable && !upgradeTooExpensive)
        {
            row.Q<Button>("upgradeBtn").RemoveFromClassList("disabled");
            row.Q<TextElement>("upgradeCost").text = stat.upgradeCost.ToString();
        }
        else
        {
            row.Q<Button>("upgradeBtn").AddToClassList("disabled");
            row.Q<TextElement>("upgradeCost").text = stat.upgradeCost.ToString();

        }

        // Update the upgrade slots
        List<VisualElement> upgradeSlots = row.Query<VisualElement>(className: "upgradeSlot").ToList();
        int i = 0;
        foreach (VisualElement slot in upgradeSlots)
        {
            // Clone the template
            VisualElement upgradeSlot = upgradeSlotTemplate.CloneTree();
            upgradeSlot.style.flexGrow = 1; // Hack to fix template containeer not respecting child styles

            // If upgrade has ben used mark it by add the slotUsed class, otherwise remove class
            if (stat.upgradesUsed > i)
            {
                slot.AddToClassList("slotUsed");
            }
            else
            {
                slot.RemoveFromClassList("slotUsed");
            }

            i++;
        }
    }


    private void OnFinish()
    {
        // Go Back to the Play state
        PlayerStatsManager.Instance.points += totalUpgradesMade * xpPerUpergrade;
        confirmAudio.Play();
        StartCoroutine(LoadPlayScene());
    }

    IEnumerator LoadPlayScene()
    {
        // Pause 1 second and then load scene
        yield return new WaitForSeconds(confirmAudio.clip.length + 0.5f);
        SceneManager.LoadScene("Play");
    }
}