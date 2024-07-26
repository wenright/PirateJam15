using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitEffectPrefab;

    private GameObject source;
    private float damage;
    private Rigidbody2D rb;
    
    public void Init(float speed, float newDamage, GameObject source)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;

        damage = newDamage;
        this.source = source;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.Damage(damage, damageable.transform.position, gameObject, Damageable.DamageType.DEFAULT);
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
