using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    private UpgradeData data;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Buy);
    }

    public void SetData(UpgradeData upgradeData)
    {
        data = upgradeData;
        // TODO set text desc/cost
    }
    
    private void Buy()
    {
        if (!data)
        {
            Debug.LogError("Tried to buy an upgrade before data was intialized", gameObject);
            return;
        }

        if (UpgradeController.Instance.GetGold() < data.cost)
        {
            Debug.LogWarning("Tried to buy an upgrade without enough money", gameObject);
            return;
        }
        
        UpgradeController.Instance.AddGold(-data.cost);
        UpgradeController.Instance.AddUpgrade(data);
        button.interactable = false;
    }
}
