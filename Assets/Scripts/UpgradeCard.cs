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
    public GameObject soldOutText;

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
    
    public void SetWizardData(UpgradeData upgradeData)
    {
        data = upgradeData;

        nameText.text = upgradeData.newWizardSpell.name + " wizard";
        Destroy(rarityText);
        string text = upgradeData.newWizardSpell.damage + " dmg";
        if (upgradeData.newWizardSpell.numProjectiles > 1)
        {
            text += " x " + upgradeData.newWizardSpell.numProjectiles;
        }
        if (upgradeData.newWizardSpell.onHitStatusEffects.Count > 0)
        {
            text += "\n+" + upgradeData.newWizardSpell.onHitStatusEffects[0].stacks + " " + upgradeData.newWizardSpell.onHitStatusEffects[0].name + "/s";

            switch (upgradeData.newWizardSpell.onHitStatusEffects[0].type)
            {
                case Damageable.DamageType.ICE:
                    text += "\nslows monsters";
                    break;
                case Damageable.DamageType.POISON:
                    text += " \nweakens monsters";
                    break;
            }
        }
        descriptionText.text = text;

        if (GameController.Instance.nightCount == 0)
        {
            costText.enabled = false;
        } 
        else
        {
            costText.text = upgradeData.cost + "G";
        }
    }
    
    private void Buy()
    {
        if (!data)
        {
            Debug.LogError("Tried to buy an upgrade before data was intialized", gameObject);
            return;
        }
        
        // Special case for first wizard
        if (GameController.Instance.nightCount == 0)
        {
            UpgradeController.Instance.AddUpgrade(data);
            GameController.Instance.SwitchState(GameController.State.NIGHTTIME);
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
        soldOutText.SetActive(true);
    }
}
