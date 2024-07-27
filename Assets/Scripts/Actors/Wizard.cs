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
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        projectileInstance.GetComponent<Projectile>().SetData(spellData, gameObject);
        
        foreach (SpriteRenderer spriteRenderer in projectileInstance.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.sprite = spellData.projectileSprite;
        }
        
        Utils.PlayOneShot(spellData.attackSound, transform.position);
    }
}
