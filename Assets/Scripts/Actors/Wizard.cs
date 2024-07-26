using UnityEngine;

public class Wizard : MonoBehaviour
{
    private Searchlight searchlight;
    
    [ReadOnly] public float lastFireTime = 0.0f;
    public SpellData spellData;

    private void Start()
    {
        searchlight = FindObjectOfType<Searchlight>();
    }

    private void Update()
    {
        if (lastFireTime + spellData.attackCooldown < Time.time)
        {
            Fire();
        }
    }

    public void Fire()
    {
        Monster targetMonster = searchlight.GetNearestTarget();

        if (targetMonster == null)
        {
            return;
        }
        
        lastFireTime = Time.time;

        float angle = Utils.AngleToPoint(transform.position, targetMonster.transform.position);
        GameObject projectileInstance = Instantiate(spellData.projectilePrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        projectileInstance.GetComponent<Projectile>().SetData(spellData, gameObject);
        
        Utils.PlayOneShot(spellData.attackSound, transform.position);
    }
}
