using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth = 100;
    public int moneyGiven = 2;
    public int xpGiven = 8;
    [ReadOnly] public float health = 0;
    private GameObject damageTextPrefab;
    
    public enum DamageType
    {
        DEFAULT,
        CRIT,
        POISON,
        ICE,
        FIRE,
        BLEED,
        LIGHT,
    }

    private void Start()
    {
        health = maxHealth;
        
        damageTextPrefab = Resources.Load<GameObject>("DamageText");
    }

    public void Damage(float damage, Vector2 position, GameObject source, DamageType damageType)
    {
        health -= damage;

        SpawnDamageText(damage, position, damageType);

        if (source)
        {
            source.GetComponent<DPSTracker>().Track(damage);
        }
        
        if (health <= 0)
        {
            UpgradeController.Instance.AddGold(moneyGiven);

            foreach (Wizard wizard in FindObjectsByType<Wizard>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            {
                wizard.AddXp(xpGiven);
            }

            // TODO death effect
            Destroy(gameObject);
        }
    }

    public void SpawnDamageText(float damage, Vector2 position, DamageType damageType)
    {
        // Check for existing damage text
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 1.0f, LayerMask.GetMask("DamageText"));

        foreach (Collider2D collider in colliders)
        {
            DamageText existingDamageText = collider.GetComponent<DamageText>();

            if (existingDamageText == null
                || !GameObject.ReferenceEquals(existingDamageText.damagee, this)
                || existingDamageText.damagee != this
                || existingDamageText.damageType != damageType) { continue; }
            
            existingDamageText.UpdateDamage(damage, position);
            return;
        }

        // TODO could maybe set damage text color/opacity to be less for other players' damage
        GameObject textInstance = Instantiate(damageTextPrefab, position, Quaternion.identity, GameController.Instance.damageTextParent);
        textInstance.GetComponent<DamageText>().SetDamage(damage, this, damageType);
    }

    public bool IsAlive()
    {
        return health > 0;
    }
}
