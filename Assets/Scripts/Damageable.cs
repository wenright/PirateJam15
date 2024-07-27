using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth = 100;
    [ReadOnly] public float health = 0;
    private int xpGiven = 10;
    private int moneyGiven = 2;
    private GameObject damageTextObject;
    
    public enum DamageType
    {
        DEFAULT,
        CRIT,
        POISON,
        ICE,
        FIRE,
        BLEED,
    }

    private void Start()
    {
        health = maxHealth;
        
        damageTextObject = Resources.Load<GameObject>("DamageText");
    }

    public void Damage(float damage, Vector2 position, GameObject source, DamageType damageType)
    {
        health -= damage;

        if (health <= 0)
        {
            // TODO on death effects, money gain etc.
            Destroy(gameObject);
        }

        SpawnDamageText(damage, position, damageType);
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
        GameObject textInstance = Instantiate(damageTextObject, position, Quaternion.identity, GameController.Instance.damageTextParent);
        textInstance.GetComponent<DamageText>().SetDamage(damage, this, damageType);
    }
}
