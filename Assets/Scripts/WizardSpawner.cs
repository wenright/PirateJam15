using System.Collections.Generic;
using UnityEngine;

public class WizardSpawner : MonoBehaviour
{
    public static WizardSpawner Instance;
    
    public List<Wizard> wizards = new();
    public GameObject wizardPrefab;
    public SpellData defaultSpell;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        SpawnWizard(defaultSpell);
    }

    public void SpawnWizard(SpellData spellData)
    {
        GameObject wizardInstance = Instantiate(wizardPrefab, transform.position, Quaternion.identity, transform);
        Wizard wizard = wizardInstance.GetComponent<Wizard>();
        wizard.spellData = spellData;
        wizards.Add(wizard);
    }
}
