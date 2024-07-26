using UnityEngine;

public class Wizard : MonoBehaviour
{
    private Monster target;
    private Searchlight searchlight;
    
    [ReadOnly] public float lastFireTime = 0.0f;
    public SpellData spellData;

    private void Start()
    {
        searchlight = FindObjectOfType<Searchlight>();
    }

    private void Update()
    {
        if (target)
        {
            if (!searchlight.IsTargetVisible(target))
            {
                target = null;
                return;
            }
            
            if (lastFireTime + spellData.attackCooldown < Time.time)
            {
                Fire();
            }
        }
        else
        {
            target = searchlight.GetRandomTarget();
        }
    }

    public void Fire()
    {
        lastFireTime = Time.time;

        float angle = Utils.AngleToPoint(transform.position, target.transform.position);
        GameObject projectileInstance = Instantiate(spellData.projectilePrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        projectileInstance.GetComponent<Projectile>().SetData(spellData, gameObject);
        
        Utils.PlayOneShot(spellData.attackSound, transform.position);
    }
}
