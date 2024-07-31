using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    [ReadOnly] public List<StatusEffectData> statusEffects = new List<StatusEffectData>();

    private float tickInterval = 0.5f;
    private float tickTimer;

    private void FixedUpdate()
    {
        tickTimer += Time.fixedDeltaTime;

        if (tickTimer >= tickInterval)
        {
            float effectDamageBonus = 1 + UpgradeController.Instance.ownedUpgrades.Where(u => u.upgradeType == UpgradeData.UpgradeType.IncreaseElementDamage).Sum(u => u.value);
            
            Damageable damageable = GetComponent<Damageable>();
            foreach (StatusEffectData effect in statusEffects.ToList().Where(effect => effect.type is Damageable.DamageType.FIRE or Damageable.DamageType.BLEED or Damageable.DamageType.POISON or Damageable.DamageType.LIGHT))
            {
                damageable.Damage(effect.value * effect.stacks * effectDamageBonus, transform.position, effect.source, effect.type);
            }

            tickTimer -= tickInterval;
        }

        // Note: Need to use ToList() to avoid modifying the list while iterating over it
        foreach (StatusEffectData effect in statusEffects.ToList().Where(effect => effect.hasDuration))
        {
            effect.duration -= Time.fixedDeltaTime;

            if (effect.duration <= 0f)
            {
                statusEffects.Remove(effect);
            }
        }
    }

    internal void AddStatusEffect(List<StatusEffectData> effects, GameObject source, int numStacks)
    {
        foreach (StatusEffectData effect in effects)
        {
            AddStatusEffect(effect, source, numStacks);
        }
    }
    
    internal void AddStatusEffect(StatusEffectData effect, GameObject source, int numStacks)
    {
        if (effect == null)
        {
            Debug.LogError("Empty status effect", gameObject);
            return;
        }

        StatusEffectData existingEffect = statusEffects.Find(e => e.name == effect.name);
        numStacks += (int) Mathf.Round(UpgradeController.Instance.ownedUpgrades.Where(u => u.upgradeType == UpgradeData.UpgradeType.IncreaseElementStacks).Sum(u => u.value));

        if (existingEffect != null)
        {
            if (existingEffect.isStackable)
            {
                existingEffect.stacks += numStacks;
            }

            existingEffect.duration = effect.duration;
        }
        else
        {
            StatusEffectData newEffect = Instantiate(effect);
            newEffect.name = effect.name;
            newEffect.source = source;
            newEffect.stacks = numStacks; // TODO note that this means anything with a non-one default stack count getes overridden
            statusEffects.Add(newEffect);
        }
    }

    internal void DecrementStacks(Damageable.DamageType type)
    {
        StatusEffectData existingEffect = statusEffects.Find(e => e.type == type);

        if (existingEffect != null)
        {
            if (existingEffect.isStackable)
            {
                existingEffect.stacks--;
            }

            if (existingEffect.stacks > 0)
            {
                return;
            }
        }
        
        RemoveStatusEffect(type);
    }

    // Removes all status effects of type. Really just used for burning light perk
    internal void RemoveStatusEffect(Damageable.DamageType type)
    {
        statusEffects.RemoveAll(e => e.type == type);
    }
}