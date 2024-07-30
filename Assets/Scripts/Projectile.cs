using UnityEngine;

public class Projectile : MonoBehaviour
{
    [ReadOnly] public GameObject hitEffectPrefab;
    [ReadOnly] public SpellData spellData;

    private GameObject source;
    private float damageBonus = 1; 
    
    private Rigidbody2D rb;
    
    public void SetData(SpellData data, GameObject owner, float damageBonus)
    {
        spellData = data;
        source = owner;
        this.damageBonus = damageBonus;
        
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * data.projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
        {
            // Deal damage
            damageable.Damage(spellData.damage * damageBonus, damageable.transform.position, source, Damageable.DamageType.DEFAULT);

            // Add status effects
            StatusEffectController statusEffectController = damageable.GetComponent<StatusEffectController>();
            if (statusEffectController && spellData.onHitStatusEffects.Count > 0)
            {
                statusEffectController.AddStatusEffect(spellData.onHitStatusEffects, source);
            }
            
            // Hit FX
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity, GameController.Instance.projectileParent);
            
            if (!spellData.piercing)
            {
                Destroy(gameObject);
            }
        }
    }
}
