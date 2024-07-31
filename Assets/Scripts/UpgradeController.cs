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
    public TMP_Text shopTitle;
    public Button leaveShopButton; 
    public Button rerollShopButton;
    public TMP_Text rerollCostText;
    public int defaultRerollCost = 3;
    public int rerollCost;
    public int rerollCostIncrement = 2;

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
        
        // add upgrades to escape menu
        if (upgradeData.upgradeType != UpgradeData.UpgradeType.Heal && upgradeData.upgradeType != UpgradeData.UpgradeType.AddWizard)
        {
            UIController.Instance.upgradeList.text += "\n" + upgradeData.displayName;
        }
    }

    public void RefreshWizardShop()
    {
        ClearShop();
        
        shopTitle.text = GameController.Instance.nightCount == 0 ? "Pick a sarting wizard" : "Hire a wizard";
        
        int numWizardsInShop = 3;
        List<SpellData> previousSpells = new List<SpellData>();
        for (int i = 0; i < numWizardsInShop; i++)
        {
            GameObject cardInstance = Instantiate(shopUpgradeCardPrefab, UIController.Instance.shopUpgradeCardParent);
            
            SpellData randomSpell = possibleSpells[Random.Range(0, possibleSpells.Count)];
            while (previousSpells.Contains(randomSpell))
            {
                randomSpell = possibleSpells[Random.Range(0, possibleSpells.Count)];
            }

            UpgradeData wizardUpgradeDataInstance = UpgradeData.Instantiate(wizardUpgradeData);
            wizardUpgradeDataInstance.newWizardSpell = randomSpell;
            cardInstance.GetComponent<UpgradeCard>().SetWizardData(wizardUpgradeDataInstance);
            
            previousSpells.Add(randomSpell);
        }
    }

    public void RefreshShop()
    {
        ClearShop();

        shopTitle.text = "Buy upgrades";
        
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
        rerollShopButton.gameObject.SetActive(GameController.Instance.nightCount != 0);
        leaveShopButton.gameObject.SetActive(GameController.Instance.nightCount != 0);
        
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
            AddGold(-rerollCost);
            RefreshShop();
            
            rerollCost += rerollCostIncrement;
            rerollCostText.text = rerollCost.ToString();
        }
    }
}
