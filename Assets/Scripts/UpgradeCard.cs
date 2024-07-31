using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    private UpgradeData data;
    private Button button;

    public TMP_Text nameText;
    public TMP_Text rarityText;
    public TMP_Text descriptionText;
    public TMP_Text costText;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Buy);
    }

    public void SetData(UpgradeData upgradeData)
    {
        data = upgradeData;

        nameText.text = upgradeData.displayName;
        rarityText.color = UpgradeData.rarityColors[data.rarity];
        rarityText.text = data.rarity.ToString();
        descriptionText.text = upgradeData.description;
        costText.text = upgradeData.cost + "G";
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
