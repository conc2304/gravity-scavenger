using System;
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
    public float upgradeCostScale;
    public bool isUpgradable = true;
    public bool isDowngradable = false;
    private readonly float costScale = 1f; // how much the cost goes up between upgrades

    public UpgradableStat(string name, float baseVal, float maxVal, int maxUpgrades, float initialUpgradeCost)
    {
        statName = name;
        baseValue = baseVal;
        maxValue = maxVal;
        this.maxUpgrades = maxUpgrades;
        upgradeIncrement = (maxVal - baseVal) / maxUpgrades;
        currentValue = baseVal;
        upgradesUsed = 0;
        upgradeCostScale = initialUpgradeCost;
    }

    public bool Upgrade()
    {
        if (upgradesUsed < maxUpgrades)
        {
            upgradesUsed++;
            currentValue += upgradeIncrement;
            upgradeCostScale += costScale;
            isUpgradable = upgradesUsed < maxUpgrades;
            return true;
        }
        else
        {
            isUpgradable = false;
            Debug.LogWarning("Maximum upgrades reached for " + statName);
            return false;

        }
    }

    public bool Downgrade()
    {
        if (upgradesUsed > 0)
        {
            upgradesUsed--;
            currentValue -= upgradeIncrement;
            upgradeCostScale -= costScale;
            isDowngradable = upgradesUsed > 0;
            return true;
        }
        else
        {
            Debug.LogWarning("Minimum upgrades reached for " + statName);
            isDowngradable = false;
            return false;
        }
    }

    public float GetUpgradeCost()
    {
        return Mathf.Round(upgradeCostScale * (1 + upgradesUsed));
    }

    public float GetDowngradCost()
    {
        return Mathf.Round(upgradeCostScale / (1 + upgradesUsed));
    }
}
