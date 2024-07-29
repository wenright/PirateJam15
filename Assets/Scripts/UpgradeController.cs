using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    public static UpgradeController Instance;
    
    [ReadOnly] public List<UpgradeData> ownedUpgrades = new();
    [ReadOnly] public List<UpgradeData> possibleUpgrades = new();

    public GameObject shopUpgradeCardPrefab;
    public Button leaveShopButton; 

    private int gold;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        possibleUpgrades.AddRange(Resources.LoadAll<UpgradeData>("Data/Upgrades"));
        RefreshShop();
        
        leaveShopButton.onClick.AddListener(LeaveShop);
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UIController.Instance.goldText.text = gold.ToString();
    }

    public int GetGold()
    {
        return gold;
    }

    public void AddUpgrade(UpgradeData upgradeData)
    {
        ownedUpgrades.Add(upgradeData);
    }

    public void RefreshShop()
    {
        int numItemsInShop = 3;
        for (int i = 0; i < numItemsInShop; i++)
        {
            GameObject cardInstance = Instantiate(shopUpgradeCardPrefab, UIController.Instance.shopUpgradeCardParent);
            cardInstance.GetComponent<UpgradeCard>().SetData(possibleUpgrades[Random.Range(0, possibleUpgrades.Count)]);
            // TODO do I need to refresh UI to get horizontal scroll thing to work
        }
    }

    private void LeaveShop()
    {
        GameController.Instance.SwitchState(GameController.State.NIGHTTIME);   
    }
}
