using UnityEngine;
using IngameDebugConsole;

public class ConsoleCommands : MonoBehaviour
{
    [ConsoleMethod("motherlode", "Gives you a bunch of money")]
    public static void Motherlode()
    {
        Debug.Log("Here's some money");
        UpgradeController.Instance.AddGold(999);
    }

    [ConsoleMethod("buy", "Buys the specified upgrade")]
    public static void Buy(UpgradeData.UpgradeType type)
    {
        Debug.Log("Buying " + type);
        
        UpgradeData[] upgrades = Resources.LoadAll<UpgradeData>("Data/Upgrades");
        foreach (var upgrade in upgrades)
        {
            if (upgrade.upgradeType == type)
            {
                UpgradeController.Instance.AddUpgrade(upgrade);
                return;
            }
        }
    }

    [ConsoleMethod("xp", "Add x xp to all wizards")]
    public static void Level(int x)
    {
        Debug.Log("Adding " + x + " xp to all wizards");
        WizardSpawner.Instance.wizards.ForEach(w => w.AddXp(x));
    }
}