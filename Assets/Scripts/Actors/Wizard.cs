using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wizard : MonoBehaviour
{
    [ReadOnly] public float lastFireTime = 0.0f;
    public SpellData spellData;
    [ReadOnly] public GameObject projectilePrefab;
    [ReadOnly] public int level = 1;
    [ReadOnly] public int xp;
    [ReadOnly] public int xpNeeded;

    private Searchlight searchlight;
    private GameObject damageTextPrefab;
    
    private void Start()
    {
        projectilePrefab = Resources.Load<GameObject>("Projectile");
        damageTextPrefab = Resources.Load<GameObject>("DamageText");
        
        searchlight = FindObjectOfType<Searchlight>();
        xpNeeded = Utils.GetXpNeeded(2);
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
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.AngleAxis(angle + randomSpread, Vector3.forward), GameController.Instance.projectileParent);
        projectileInstance.GetComponent<Projectile>().SetData(spellData, gameObject);
        
        // Sets the in/out mask sprites
        foreach (SpriteRenderer spriteRenderer in projectileInstance.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.sprite = spellData.projectileSprite;
        }
        
        Utils.PlayOneShot(spellData.attackSound, transform.position);
    }

    public void AddXp(int xpAmount)
    {
        float xpMult = UpgradeController.Instance.ownedUpgrades.Where(u => u.upgradeType == UpgradeData.UpgradeType.IncreasedXp).Sum(u => u.value);
        
        xp += (int)(xpAmount * (1 + xpMult));

        while (xp >= xpNeeded)
        {
            level++;
            xpNeeded = Utils.GetXpNeeded(level+1);

            // TODO Show level up upgrades
            Debug.Log("Ding!", gameObject);
            
            // Hijacking damage text prefab to show level up
            GameObject textInstance = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, transform);
            textInstance.GetComponent<DamageText>().text.text = "LVL " + level;
            textInstance.GetComponent<DamageText>().startPos = transform.position + Vector3.up * 1.0f;
            textInstance.GetComponent<DamageText>().RefreshText(false);
        }
    }

    private void OnMouseEnter()
    {
        
    }
}
