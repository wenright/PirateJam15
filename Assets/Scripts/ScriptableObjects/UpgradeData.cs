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
}
