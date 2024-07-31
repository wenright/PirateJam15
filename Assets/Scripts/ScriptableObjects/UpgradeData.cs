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
            Rarity.Common, Color.white
        },
        {
            Rarity.Uncommon, Color.green
        },
        {
            Rarity.Rare, Color.blue
        },
        {
            Rarity.Epic, new Color(0.75f, 0, 1)
        },
        {
            Rarity.Legendary, new Color(1, 0.5f, 0)
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
