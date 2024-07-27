using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public static UpgradeController Instance;

    private int gold;
    
    private void Awake()
    {
        Instance = this;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UIController.Instance.goldText.text = gold.ToString();
    }
}
