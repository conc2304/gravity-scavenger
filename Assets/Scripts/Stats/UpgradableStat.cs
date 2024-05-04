using UnityEngine;

public class UpgradableStat
{
    public string statName;
    public float baseValue;
    public float maxValue;
    public int maxUpgrades;
    public float upgradeIncrement;
    public float currentValue;
    public int upgradesUsed;
    public float upgradeCost;
    public float downgradeCost;
    public bool isUpgradable = true;
    public bool isDowngradable = false;
    private readonly float costScale = 1.75f; // how much the cost goes up between upgrades

    public UpgradableStat(string name, float baseVal, float maxVal, int maxUpgrades, float initialUpgradeCost)
    {
        statName = name;
        baseValue = baseVal;
        maxValue = maxVal;
        this.maxUpgrades = maxUpgrades;
        upgradeIncrement = (maxVal - baseVal) / maxUpgrades;
        currentValue = baseVal;
        upgradesUsed = 0;
        downgradeCost = 0;
        upgradeCost = initialUpgradeCost;
    }

    public bool Upgrade()
    {
        if (isUpgradable)
        {
            downgradeCost = upgradeCost;
            upgradesUsed++;
            currentValue += upgradeIncrement;
            // Increase cost of the next upgrade 
            upgradeCost = Mathf.RoundToInt(upgradeCost * costScale);
            isUpgradable = upgradesUsed < maxUpgrades;
            isDowngradable = upgradesUsed > 0;

            return true;
        }
        else
        {
            Debug.LogWarning("Maximum upgrades reached for " + statName);
            return false;
        }
    }

    public bool Downgrade()
    {
        if (isDowngradable)
        {
            upgradeCost = downgradeCost;
            upgradesUsed--;
            currentValue -= upgradeIncrement;
            // Decrease cost of the next downgrade 
            downgradeCost = Mathf.RoundToInt(upgradeCost / costScale);
            isDowngradable = upgradesUsed > 0;
            isUpgradable = upgradesUsed < maxUpgrades;

            return true;
        }
        else
        {
            Debug.LogWarning("Minimum upgrades reached for " + statName);
            return false;
        }
    }

    public void Reset()
    {
        currentValue = baseValue;
        upgradesUsed = 0;
        upgradeCost = Mathf.RoundToInt(upgradeCost / Mathf.Pow(costScale, upgradesUsed));
        downgradeCost = 0;
        isUpgradable = true;
        isDowngradable = false;
    }
}
