using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider healthSlider;
    private Damageable damageable;

    private void Start()
    {
        damageable = GetComponent<Damageable>();
        
        healthSlider.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (!Mathf.Approximately(damageable.health / damageable.maxHealth, 1))
        {
            healthSlider.gameObject.SetActive(true);
            healthSlider.value = damageable.health / damageable.maxHealth;
        }
    }
}
