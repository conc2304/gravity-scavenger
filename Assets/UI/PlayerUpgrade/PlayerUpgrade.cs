using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;


// Persistence Layer of Player Stats
public class PlayerUpgrade : MonoBehaviour
{

    public VisualTreeAsset rowTemplate;
    public VisualTreeAsset upgradeSlotTemplate;

    private UIDocument playerUpgradeDocument;
    private VisualElement parentContainer;

    private TextElement availablePartsElem;
    private TextElement totalQtyElem;

    private float totalParts;

    private int totalUpgrades;
    private Button upgradeBtn;
    readonly Dictionary<string, VisualElement> rows = new();

    private void Start()
    {
        totalUpgrades = PlayerStatsManager.MaxUpgrades;

        playerUpgradeDocument = GetComponent<UIDocument>();
        parentContainer = playerUpgradeDocument.rootVisualElement.Q("content");

        // Set the UI for available points and total used
        availablePartsElem = playerUpgradeDocument.rootVisualElement.Q<TextElement>("availableQty");
        availablePartsElem.text = PlayerStatsManager.Instance.parts.ToString();
        totalQtyElem = playerUpgradeDocument.rootVisualElement.Q<TextElement>("totalQty");
        totalQtyElem.text = totalParts.ToString();

        // Loop through each upgradable stat
        // Add a upgradeRow template for each stat
        foreach (UpgradableStat stat in PlayerStatsManager.Instance.GetAllUpgradableStats())
        {
            // Clone the template
            rows[stat.statName] = rowTemplate.CloneTree();
            VisualElement upgradeStatusContainer = rows[stat.statName].Q<VisualElement>("statusBar");

            // Populate the row with data from the upgradable stat
            rows[stat.statName].Q<TextElement>("statName").text = stat.statName;
            rows[stat.statName].Q<TextElement>("downgradeCost").text = stat.upgradeCost.ToString();
            rows[stat.statName].Q<TextElement>("upgradeCost").text = stat.downgradeCost.ToString();

            // Set click callbacks
            rows[stat.statName].Q<Button>("downgradeBtn").clicked += () => OnDowngradeStat(stat, rows[stat.statName]);
            rows[stat.statName].Q<Button>("upgradeBtn").clicked += () => OnUpgradeStat(stat, rows[stat.statName]);

            //  Add upgrade slots to UI
            for (int i = 0; i < totalUpgrades; i++)
            {
                // Clone the template
                VisualElement upgradeSlot = upgradeSlotTemplate.CloneTree();
                upgradeSlot.style.flexGrow = 1; // Hack to fix template containeer not respecting child styles

                // If upgrade has ben used mark it by add the slotUsed class
                if (stat.upgradesUsed > i)
                {
                    upgradeSlot.AddToClassList("slotUsed");
                }

                // Add component to parent container
                upgradeStatusContainer.Add(upgradeSlot);
            }


            // Add the row to the parent container
            parentContainer.Add(rows[stat.statName]);
            UpdateRowUI(stat, rows[stat.statName]);
        }

        upgradeBtn = playerUpgradeDocument.rootVisualElement.Q("confirmButton") as Button;
        upgradeBtn.clicked += () => OnFinish();
    }

    private void OnUpgradeStat(UpgradableStat stat, VisualElement statRow)
    {

        Debug.Log("Upgrade: " + stat.statName);
        float cost = stat.upgradeCost;

        // Only allow upgrade if player has enoug to cover cost
        if (cost < PlayerStatsManager.Instance.parts)
        {
            bool success = stat.Upgrade();

            // If we successfully upgraded the stat, then update the UI
            if (success)
            {
                // Update points total
                totalParts += cost;
                PlayerStatsManager.Instance.parts -= cost;

                foreach (UpgradableStat uStat in PlayerStatsManager.Instance.GetAllUpgradableStats())
                {
                    UpdateRowUI(uStat, rows[uStat.statName]);
                }
            }
        }
        else
        {
            Debug.Log("Too expensive: " + cost + " vs " + PlayerStatsManager.Instance.parts);
        }

    }

    private void OnDowngradeStat(UpgradableStat stat, VisualElement statRow)
    {
        Debug.Log("Downgrade: " + stat.statName);

        float cost = stat.downgradeCost;
        bool success = stat.Downgrade();

        // If we successfully upgraded the stat, then update the UI
        if (success)
        {
            // Update points total
            totalParts -= cost;
            totalQtyElem.text = totalParts.ToString();
            PlayerStatsManager.Instance.parts += cost;
            availablePartsElem.text = PlayerStatsManager.Instance.parts.ToString();

            // We update all rows so that if upgrads are out of the price range the UI reflects that
            foreach (UpgradableStat uStat in PlayerStatsManager.Instance.GetAllUpgradableStats())
            {
                UpdateRowUI(uStat, rows[uStat.statName]);
            }
        }
    }

    private void UpdateRowUI(UpgradableStat stat, VisualElement row)
    {
        Debug.Log("Update UI Row: " + stat.statName);
        // Update the "shoping cart" totals
        availablePartsElem.text = PlayerStatsManager.Instance.parts.ToString();
        totalQtyElem.text = totalParts.ToString();

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
            row.Q<TextElement>("upgradeCost").text = "X";
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
        SceneManager.LoadScene("Play");
    }
}