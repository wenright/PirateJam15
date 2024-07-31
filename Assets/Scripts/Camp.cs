using TMPro;
using UnityEngine;

public class Camp : MonoBehaviour
{
    public static Camp Instance;
    
    public float health = 100;
    
    public TMP_Text campHPText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        campHPText.text = health.ToString();
    }
    
    public void Damage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Debug.Log("Camp has been destroyed :(");
        }

        campHPText.text = health.ToString();
    }

    public void Heal(float amount)
    {
        health += amount;
        campHPText.text = health.ToString();
    }
}
