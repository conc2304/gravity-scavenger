using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


// Persistence Layer of Player Stats
public class PlayerUpgrade : MonoBehaviour
{

    public VisualTreeAsset rowTemplate;
    public VisualTreeAsset upgradeSlotTemplate;

    private UIDocument playerUpgradeDocument;
    private VisualElement parentContainer;

    private TextElement availableQtyElem;
    private TextElement totalQtyElem;

    private float totalParts = 0;
    private int totalUpgrades;

    private void Start()
    {
        totalUpgrades = PlayerStatsManager.MaxUpgrades;

        playerUpgradeDocument = GetComponent<UIDocument>();
        parentContainer = playerUpgradeDocument.rootVisualElement.Q("content");

        // Set the UI for available points and total used
        availableQtyElem = playerUpgradeDocument.rootVisualElement.Q<TextElement>("availableQty");
        availableQtyElem.text = PlayerStatsManager.Instance.parts.ToString();
        totalQtyElem = playerUpgradeDocument.rootVisualElement.Q<TextElement>("totalQty");
        totalQtyElem.text = totalParts.ToString();


        // Loop through each upgradable stat
        foreach (UpgradableStat stat in PlayerStatsManager.Instance.GetAllUpgradableStats())
        {
            // Clone the template
            VisualElement row = rowTemplate.CloneTree();
            VisualElement upgradeStatusContainer = row.Q<VisualElement>("statusBar");

            // Populate the row with data from the upgradable stat
            row.Q<TextElement>("statName").text = stat.statName;
            row.Q<TextElement>("downgradeCost").text = stat.GetUpgradeCost().ToString();
            row.Q<TextElement>("upgradeCost").text = stat.GetUpgradeCost().ToString();

            // Set click callbacks
            row.Q<Button>("downgradeBtn").clicked += () => OnDowngradeStat(stat);
            row.Q<Button>("upgradeBtn").clicked += () => OnUpgradeStat(stat);

            //  Add upgrade slots to UI
            for (int i = 0; i < totalUpgrades; i++)
            {
                // Clone the template
                VisualElement upgradeSlot = upgradeSlotTemplate.CloneTree();
                upgradeSlot.style.flexGrow = 1; // Hack to fix template containeer not respecting child styles
                // Add upgrade slot to parent container
                upgradeStatusContainer.Add(upgradeSlot);
            }

            // Add the row to the parent container
            parentContainer.Add(row);
        }
    }

    private void OnUpgradeStat(UpgradableStat stat)
    {

        Debug.Log("Upgrade: " + stat.statName);
        // bool status = stat.Upgrade();


    }

    private void OnDowngradeStat(UpgradableStat stat)
    {
        Debug.Log("Upgrade: " + stat.statName);

    }

    private void OnCancel()
    {
        Debug.Log("Cancel Upgrade");
        // SceneManager.LoadScene("Play");

    }

    private void OnConfirm()
    {

        SceneManager.LoadScene("Home");

    }


}