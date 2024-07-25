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
    }

    public void Damage(float damage)
    {
        Debug.Log(damage);
        health -= damage;

        if (health <= 0)
        {
            // TODO on death effects, money gain etc.
            Destroy(gameObject);
        }
    }
}
