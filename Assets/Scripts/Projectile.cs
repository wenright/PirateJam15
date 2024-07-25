using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitEffectPrefab;
    
    private float damage;
    private Rigidbody2D rb;
    
    public void Init(float speed, float newDamage)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;

        damage = newDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.Damage(damage);
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
