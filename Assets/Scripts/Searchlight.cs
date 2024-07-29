using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Searchlight : MonoBehaviour
{
    private List<Monster> visibleMonsters = new List<Monster>();
    private float rotateSpeed = 75f;

    private void Update()
    {
        // TODO do we want mouse controls?
        // transform.rotation = Quaternion.LookRotation(Vector3.forward, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

        float rotationDirection = -Input.GetAxisRaw("Horizontal");
        float speedBonus = 1 + UpgradeController.Instance.ownedUpgrades.Where(u => u.upgradeType == UpgradeData.UpgradeType.TurnSpeed).Sum(u => u.value);
        transform.rotation *= Quaternion.Euler(0, 0, rotationDirection * rotateSpeed * speedBonus * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Monster otherMonster = other.GetComponent<Monster>();
        if (otherMonster)
        {
            visibleMonsters.Add(otherMonster);
            otherMonster.isIlluminated = true;

            float burnAmount = UpgradeController.Instance.ownedUpgrades.Where(u => u.upgradeType == UpgradeData.UpgradeType.BurningLight).Sum(u => u.value);

            if (burnAmount > 0)
            {
                StatusEffectData lightBurnData = ScriptableObject.CreateInstance<StatusEffectData>();
                lightBurnData.type = Damageable.DamageType.LIGHT;
                lightBurnData.duration = 99999999;
                lightBurnData.stacks = 1;
                lightBurnData.value = burnAmount;
                otherMonster.GetComponent<StatusEffectController>().AddStatusEffect(lightBurnData, null);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Monster otherMonster = other.GetComponent<Monster>();
        if (otherMonster)
        {
            visibleMonsters.Remove(otherMonster);
            otherMonster.isIlluminated = false;

            otherMonster.GetComponent<StatusEffectController>().RemoveStatusEffect(Damageable.DamageType.LIGHT);
        }
    }

    public Monster GetNearestTarget()
    {
        if (visibleMonsters == null || visibleMonsters.Count == 0) return null;
        
        return visibleMonsters.ToList().OrderBy(m => Vector3.Distance(transform.position, m.transform.position)).First();
    }
    
    public bool IsTargetVisible(Monster target)
    {
        return visibleMonsters.Contains(target);
    }
}
