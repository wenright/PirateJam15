using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Searchlight : MonoBehaviour
{
    public static Searchlight Instance;
    
    public List<Monster> visibleMonsters = new List<Monster>();
    [ReadOnly] public float rotateSpeed = 75f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // TODO do we want mouse controls?
        // transform.rotation = Quaternion.LookRotation(Vector3.forward, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

        float rotationDirection = -Input.GetAxisRaw("Horizontal");
        float speedBonus = 1 + UpgradeController.Instance.ownedUpgrades.Where(u => u.upgradeType == UpgradeData.UpgradeType.TurnSpeed).Sum(u => u.value);
        transform.rotation *= Quaternion.Euler(0, 0, rotationDirection * rotateSpeed * speedBonus * Time.deltaTime);
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
