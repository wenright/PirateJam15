using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeData", menuName = "Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary };
    public Rarity rarity;
    public static Dictionary<Rarity, Color> rarityColors = new Dictionary<Rarity, Color>()
    {
        {
            Rarity.Common, new Color(184/255.0f, 181/255.0f, 185/255.0f)
        },
        {
            Rarity.Uncommon, new Color(194/255.0f, 211/255.0f, 104/255.0f)
        },
        {
            Rarity.Rare, new Color(75/255.0f, 128/255.0f, 202/255.0f)
        },
        {
            Rarity.Epic, new Color(180/255.0f, 82/255.0f, 82/255.0f)
        },
        {
            Rarity.Legendary, new Color(207/255.0f, 138/255.0f, 203/255.0f)
        }
    };

    public string displayName;
    public string description;
    public int cost;
    public float value;

    public enum UpgradeType
    {
        DazzlingLight,
        BurningLight,
        IncreasedXp,
        TurnSpeed,
        AddWizard,
        Heal,
        IncreaseElementDamage,
        IncreaseElementStacks,
        IncreaseCritChance,
        IncreaseAttackSpeed,
        IncreaseDamageToImpairedEnemies,
        AddInterest,
    }
    public UpgradeType upgradeType;

    [ShowIf("upgradeType", UpgradeType.AddWizard)] public SpellData newWizardSpell;
    [ShowIf("upgradeType", UpgradeType.IncreaseElementDamage)] public Damageable.DamageType elementToIncreaseDamage;
    [ShowIf("upgradeType", UpgradeType.IncreaseElementStacks)] public Damageable.DamageType elementToIncreaseStacks;
}
