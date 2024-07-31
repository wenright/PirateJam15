using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Camp : MonoBehaviour
{
    public static Camp Instance;
    
    public float health = 100;
    
    public TMP_Text campHPText;
    public Image hurtFlash;

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
            GameController.Instance.GameOver();
        }

        campHPText.text = Mathf.RoundToInt(health).ToString();
        
        hurtFlash.DOFade(0.15f, 0f);
        hurtFlash.DOFade(0, 0.3f);
    }

    public void Heal(float amount)
    {
        health += amount;
        campHPText.text = Mathf.RoundToInt(health).ToString();
    }
}
