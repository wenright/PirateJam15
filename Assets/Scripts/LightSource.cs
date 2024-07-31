using System.Linq;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    private StatusEffectData lightburnStatusEffect;
    
    private void Start()
    {
        lightburnStatusEffect = Resources.Load<StatusEffectData>("Data/StatusEffects/Lightburn");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Monster otherMonster = other.GetComponent<Monster>();
        if (otherMonster)
        {
            Searchlight.Instance.visibleMonsters.Add(otherMonster);
            otherMonster.isIlluminated = true;

            float burnAmount = UpgradeController.Instance.ownedUpgrades.Where(u => u.upgradeType == UpgradeData.UpgradeType.BurningLight).Sum(u => u.value);

            if (burnAmount > 0)
            {
                otherMonster.GetComponent<StatusEffectController>().AddStatusEffect(lightburnStatusEffect, null, 1);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Monster otherMonster = other.GetComponent<Monster>();
        if (otherMonster)
        {
            Searchlight.Instance.visibleMonsters.Remove(otherMonster);
            otherMonster.isIlluminated = false;

            otherMonster.GetComponent<StatusEffectController>().DecrementStacks(Damageable.DamageType.LIGHT);
        }
    }
}
