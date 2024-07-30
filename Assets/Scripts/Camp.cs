using UnityEngine;

public class Camp : MonoBehaviour
{
    public static Camp Instance;
    
    public float health = 100;

    private void Awake()
    {
        Instance = this;
    }
    
    public void Damage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Debug.Log("Camp has been destroyed :(");
        }
    }

    public void Heal(float amount)
    {
        health += amount;
    }
}
