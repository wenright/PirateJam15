using System.Linq;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Vector3 target = Vector3.zero;
    private float speed = 1.0f;
    private float attackDistance = 1f;
    private float attackDamage = 10.0f;
    private float lastAttackTimeSeconds;
    private float attackDelaySeconds = 0.25f;
    public bool isIlluminated;
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target) > attackDistance)
        {
            rb.velocity = (target - transform.position).normalized * speed;
            if (isIlluminated)
            {
                // TODO could pull a value, so it can be upgraded. Might require using a StatusEffect which would require changing StatusEffects to not always deal damage
                rb.velocity *= 0.75f;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;

            if (lastAttackTimeSeconds + attackDelaySeconds <= Time.time)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        lastAttackTimeSeconds = Time.time;
        Debug.Log("Attacking tower for " + attackDamage + " damage");
    }
}
