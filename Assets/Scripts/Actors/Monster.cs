using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public bool isIlluminated;
    public AudioClip attackSound;
    
    private Vector3 target = Vector3.zero;
    private float speed = 1.0f;
    private float attackDistance = 1.5f;
    private float attackDamage = 5.0f;
    private float lastAttackTimeSeconds;
    private float attackDelaySeconds = 1.0f;
    
    private Rigidbody2D rb;
    private StatusEffectController statusEffectController;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        statusEffectController = GetComponent<StatusEffectController>();

        // TODO this is cool, but makes the eyes offset for 1/2 the monsters
        // if (transform.position.x < 0)
        // {
        //     foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
        //     {
        //         child.flipX = true;
        //     }
        //         
        // }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target) > attackDistance)
        {
            rb.velocity = (target - transform.position).normalized * speed;
            
            // Light slow
            if (isIlluminated)
            {
                int numSlowingLights = UpgradeController.Instance.ownedUpgrades.Count(u => u.upgradeType == UpgradeData.UpgradeType.DazzlingLight);
                for (int i = 0; i < numSlowingLights; i++)
                {
                    rb.velocity *= 0.75f;
                }
            }
            
            // Ice slow
            int iceCount = statusEffectController.statusEffects.Count(e => e.type == Damageable.DamageType.ICE);
            for (int i = 0; i < iceCount; i++)
            {
                rb.velocity *= 0.75f;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;

            if (lastAttackTimeSeconds + attackDelaySeconds <= Time.time && Camp.Instance.health > 0)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        lastAttackTimeSeconds = Time.time;

        int sign = (int)Mathf.Sign(transform.position.x);
        
        transform.DORotate(new Vector3(0, 0, -25 * sign), 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Utils.PlayOneShot(attackSound, transform.position);
            
            int poisonCount = statusEffectController.statusEffects.Count(e => e.type == Damageable.DamageType.POISON);
            float damage = attackDamage;
            for (int i = 0; i < poisonCount; i++)
            {
                damage *= 0.85f;
            }
            Camp.Instance.Damage(damage);
            
            transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutElastic);
        });
    }
}
