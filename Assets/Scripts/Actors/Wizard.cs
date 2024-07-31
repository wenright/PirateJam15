using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wizard : MonoBehaviour
{
    public SpriteRenderer levelUpIcon;
    [ReadOnly] public float lastFireTime;
    public SpellData spellData;
    [ReadOnly] public GameObject projectilePrefab;
    [ReadOnly] public int level = 1;
    [ReadOnly] public int xp;
    [ReadOnly] public int xpNeeded;
    [ReadOnly] public int pendingUpgrades;
    public float wanderRadius = 1.0f;
    public float wanderIntervalSeconds = 2.5f;
    public float wanderSpeed = 0.5f;

    private Searchlight searchlight;
    private GameObject damageTextPrefab;
    
    private void Start()
    {
        projectilePrefab = Resources.Load<GameObject>("Projectile");
        damageTextPrefab = Resources.Load<GameObject>("DamageText");
        
        searchlight = FindObjectOfType<Searchlight>();
        xpNeeded = Utils.GetXpNeeded(2);

        StartCoroutine(nameof(Wander));
    }

    private void Update()
    {
        float cooldown = spellData.attackCooldown * (Mathf.Lerp(1, 0.25f, Mathf.Pow(level - 1, 1.2f) / 100.0f));
        float cooldownReduction = UpgradeController.Instance.ownedUpgrades.Where(u => u.upgradeType == UpgradeData.UpgradeType.IncreaseAttackSpeed).Sum(u => u.value);
        cooldown = cooldown / (1 + cooldownReduction);
        if (lastFireTime + cooldown < Time.time)
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
        float levelDamageBonus = 1 + Mathf.Pow(level - 1, 1.7f) * 0.1f;
        float randomSpread = Random.Range(-spellData.projectileSpread / 2, spellData.projectileSpread / 2);
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.AngleAxis(angle + randomSpread, Vector3.forward), GameController.Instance.projectileParent);
        projectileInstance.GetComponent<Projectile>().SetData(spellData, gameObject, levelDamageBonus);
        
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
            
            // Hijacking damage text prefab to show level up
            GameObject textInstance = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, transform);
            textInstance.GetComponent<DamageText>().text.text = "LVL " + level;
            textInstance.GetComponent<DamageText>().startPos = transform.position + Vector3.up * 1.0f;
            textInstance.GetComponent<DamageText>().RefreshText(false);
            
            if (level % 5 == 0)
            {
                // TODO come back to this. Might not have time to do upgrading.
                // levelUpIcon.enabled = true;
                // pendingUpgrades++;
            }
        }
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            yield return new WaitForSeconds(wanderIntervalSeconds + Random.Range(0.0f, 1.0f));
            transform.DOMove(Random.insideUnitCircle * wanderRadius, wanderSpeed).SetEase(Ease.Linear);
        }
    }
}
