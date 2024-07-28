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
            Damageable damageable = GetComponent<Damageable>();
            foreach (StatusEffectData effect in statusEffects)
            {
                damageable.Damage(effect.value * effect.stacks, transform.position, effect.source, effect.type);
            }

            tickTimer -= tickInterval;
        }

        // Note: Need to use ToList() to avoid modifying the list while iterating over it
        foreach (StatusEffectData effect in statusEffects.ToList())
        {
            effect.duration -= Time.fixedDeltaTime;

            if (effect.duration <= 0f)
            {
                statusEffects.Remove(effect);
            }
        }
    }

    internal void AddStatusEffect(List<StatusEffectData> effects, GameObject source)
    {
        foreach (StatusEffectData effect in effects)
        {
            AddStatusEffect(effect, source);
        }
    }
    
    internal void AddStatusEffect(StatusEffectData effect, GameObject source)
    {
        if (effect == null)
        {
            Debug.LogError("Empty status effect", gameObject);
            return;
        }

        StatusEffectData existingEffect = statusEffects.Find(e => e.name == effect.name);

        if (existingEffect != null)
        {
            if (existingEffect.isStackable)
            {
                existingEffect.stacks++;
            }

            existingEffect.duration = effect.duration;
        }
        else
        {
            StatusEffectData newEffect = Instantiate(effect);
            newEffect.name = effect.name;
            newEffect.source = source;
            statusEffects.Add(newEffect);
        }
    }
}