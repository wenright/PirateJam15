using UnityEngine;

public class Projectile : MonoBehaviour
{
    [ReadOnly] public GameObject hitEffectPrefab;
    [ReadOnly] public SpellData spellData;

    private GameObject source;
    private Rigidbody2D rb;
    
    public void SetData(SpellData data, GameObject owner)
    {
        spellData = data;
        source = owner;
        
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * data.projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
        {
            // Deal damage
            damageable.Damage(spellData.damage, damageable.transform.position, gameObject, Damageable.DamageType.DEFAULT);

            // Add status effects
            StatusEffectController statusEffectController = damageable.GetComponent<StatusEffectController>();
            if (statusEffectController && spellData.onHitStatusEffects.Count > 0)
            {
                statusEffectController.AddStatusEffect(spellData.onHitStatusEffects);
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
