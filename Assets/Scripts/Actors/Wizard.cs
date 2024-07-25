using UnityEngine;

public class Wizard : MonoBehaviour
{
    private Monster target;
    private Searchlight searchlight;

    public GameObject projectilePrefab;
    [ReadOnly] public float lastFireTime = 0.0f;
    public float rateOfFire = 1.5f;
    public float damage = 2.0f;

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
            
            if (lastFireTime + rateOfFire < Time.time)
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
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        projectileInstance.GetComponent<Projectile>().Init(10, damage);
    }
}
