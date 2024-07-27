using UnityEngine;

public class Wizard : MonoBehaviour
{
    [ReadOnly] public float lastFireTime = 0.0f;
    public SpellData spellData;
    [ReadOnly] public GameObject projectilePrefab;

    private Searchlight searchlight;
    
    private void Start()
    {
        projectilePrefab = Resources.Load<GameObject>("Projectile");
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

        if (spellData.numProjectiles == 1)
        {
            SpawnProjectile(angle);
        }
        else
        {
            for (int i = 0; i < spellData.numProjectiles; i++)
            {
                float pct = i / (float)(spellData.numProjectiles - 1);
                float multishotSpread = spellData.multishotSpread * (pct - 0.5f);
                
                SpawnProjectile(angle + multishotSpread);
            }
        }
    }

    private void SpawnProjectile(float angle)
    {
        float randomSpread = Random.Range(-spellData.projectileSpread / 2, spellData.projectileSpread / 2);
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.AngleAxis(angle + randomSpread, Vector3.forward));
        projectileInstance.GetComponent<Projectile>().SetData(spellData, gameObject);
        
        // Sets the in/out mask sprites
        foreach (SpriteRenderer spriteRenderer in projectileInstance.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.sprite = spellData.projectileSprite;
        }
        
        Utils.PlayOneShot(spellData.attackSound, transform.position);
    }
}
