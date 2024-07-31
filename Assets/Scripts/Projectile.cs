using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [ReadOnly] public GameObject hitEffectPrefab;
    [ReadOnly] public SpellData spellData;
    [ReadOnly] public Transform homingTarget;

    private GameObject source;
    private float levelDamageBonus = 1;
    
    private Rigidbody2D rb;
    
    private void FixedUpdate()
    {
        if (spellData.homing)
        {
            if (homingTarget && Vector2.Distance(transform.position, homingTarget.position) < spellData.homingDistance)
            {
                rb.AddForce((Vector2)(homingTarget.position - transform.position).normalized * spellData.homingStrength - rb.velocity);
            }
            else
            {
                homingTarget = Utils.FindClosestByTag(transform.position, "Enemy");
            }
        }
    }
    
    public void SetData(SpellData data, GameObject owner, float levelDamageBonus)
    {
        spellData = data;
        source = owner;
        this.levelDamageBonus = levelDamageBonus;
        
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * data.projectileSpeed;

        GetComponentInChildren<SpriteRenderer>().sprite = data.projectileSprite;
        GetComponentInChildren<SpriteRenderer>().transform.localRotation = Quaternion.Euler(0, 0, data.spriteRotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
        {
            float critChance = UpgradeController.Instance.ownedUpgrades.Where(u => u.upgradeType == UpgradeData.UpgradeType.IncreaseCritChance).Sum(u => u.value);
            float critDamage = 1.0f;
            if (Random.Range(0.0f, 1.0f) >= critChance)
            {
                critDamage = 2.0f;
            }

            float affectedDamageBonus = 1;
            if (other.GetComponent<StatusEffectController>().statusEffects.Count > 0)
            {
                affectedDamageBonus += UpgradeController.Instance.ownedUpgrades.Where(u => u.upgradeType == UpgradeData.UpgradeType.IncreaseDamageToImpairedEnemies).Sum(u => u.value);
            }
            
            // Deal damage
            damageable.Damage(spellData.damage * levelDamageBonus * affectedDamageBonus * critDamage, damageable.transform.position, source, Damageable.DamageType.DEFAULT);

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
