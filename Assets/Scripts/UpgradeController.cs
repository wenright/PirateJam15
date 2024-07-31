using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    public static UpgradeController Instance;
    
    public List<UpgradeData> ownedUpgrades = new();
    public List<UpgradeData> possibleUpgrades = new();
    public UpgradeData wizardUpgradeData;
    public List<SpellData> possibleSpells = new();

    public GameObject shopUpgradeCardPrefab;
    public Button leaveShopButton; 
    public Button rerollShopButton;
    public TMP_Text rerollCostText;
    public int defaultRerollCost = 3;
    public int rerollCost;
    public int rerollCostIncrement = 1;

    private int gold;
    
    private void Awake()
    {
        Instance = this;
        
        possibleUpgrades.AddRange(Resources.LoadAll<UpgradeData>("Data/Upgrades"));
        possibleSpells.AddRange(Resources.LoadAll<SpellData>("Data/Spells"));
    }

    private void Start()
    {
        rerollCost = defaultRerollCost;
        rerollCostText.text = rerollCost.ToString();
        
        leaveShopButton.onClick.AddListener(LeaveShop);
        rerollShopButton.onClick.AddListener(RerollShop);
        
        AddGold(20);
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

        switch (upgradeData.upgradeType)
        {
            case UpgradeData.UpgradeType.AddWizard:
                WizardSpawner.Instance.SpawnWizard(upgradeData.newWizardSpell);
                break;
            case UpgradeData.UpgradeType.Heal:
                Camp.Instance.Heal(upgradeData.value);
                break;
        }
    }

    public void RefreshWizardShop()
    {
        ClearShop();
        
        int numWizardsInShop = 3;
        for (int i = 0; i < numWizardsInShop; i++)
        {
            GameObject cardInstance = Instantiate(shopUpgradeCardPrefab, UIController.Instance.shopUpgradeCardParent);
            wizardUpgradeData.newWizardSpell = possibleSpells[Random.Range(0, possibleSpells.Count)];
            cardInstance.GetComponent<UpgradeCard>().SetWizardData(wizardUpgradeData);
        }
    }

    public void RefreshShop()
    {
        ClearShop();
        
        int numItemsInShop = 3;
        for (int i = 0; i < numItemsInShop; i++)
        {
            GameObject cardInstance = Instantiate(shopUpgradeCardPrefab, UIController.Instance.shopUpgradeCardParent);
            cardInstance.GetComponent<UpgradeCard>().SetData(possibleUpgrades[Random.Range(0, possibleUpgrades.Count)]);
        }
    }

    private void LeaveShop()
    {
        if (GameController.Instance.nightCount == 0)
        {
            GameController.Instance.SwitchState(GameController.State.NIGHTTIME);   
        }
        else
        {
            if (GameController.Instance.state == GameController.State.HIRING)
            {
                GameController.Instance.SwitchState(GameController.State.SHOPPING);   
            }
            else
            {
                GameController.Instance.SwitchState(GameController.State.NIGHTTIME);   
            }
        }
    }

    private void ClearShop()
    {
        for (int i = UIController.Instance.shopUpgradeCardParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(UIController.Instance.shopUpgradeCardParent.transform.GetChild(i).gameObject);
        }
    }

    private void RerollShop()
    {
        if (rerollCost > gold)
        {
            Debug.LogWarning("Tried to reroll without enough money");
        }
        else
        {
            AddGold(rerollCost);
            RefreshShop();
            
            rerollCost += rerollCostIncrement;
            rerollCostText.text = rerollCost.ToString();
        }
    }
}
