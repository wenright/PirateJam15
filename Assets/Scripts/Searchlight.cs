using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Searchlight : MonoBehaviour
{
    public static Searchlight Instance;
    
    public Vector2 limits = new Vector2(10, 16);
    public float scaleX = 4;
    public float scaleY = 2;
    public List<Monster> visibleMonsters = new List<Monster>();
    [ReadOnly] public float rotateSpeed = 75f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GameController.Instance.state != GameController.State.NIGHTTIME) return;
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Camera movement
        Vector3 targetPosition = (transform.position + mousePos);
        targetPosition.x /= scaleX;
        targetPosition.y /= scaleY;
        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, transform.position.x - limits.x, transform.position.x + limits.x),
            Mathf.Clamp(targetPosition.y, transform.position.y - limits.y, transform.position.y + limits.y),
            -10);
        Camera.main.transform.position = targetPosition;
        
        // Rotation
        float speedBonus = 1 + UpgradeController.Instance.ownedUpgrades.Where(u => u.upgradeType == UpgradeData.UpgradeType.TurnSpeed).Sum(u => u.value);
        float rotationSpeed = rotateSpeed * speedBonus;

        Vector3 direction = (mousePos - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // float rotationDirection = -Input.GetAxisRaw("Horizontal");
        // transform.rotation *= Quaternion.Euler(0, 0, rotationDirection * rotateSpeed * speedBonus * Time.deltaTime);
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
