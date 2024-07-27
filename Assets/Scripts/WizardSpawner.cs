using System.Collections.Generic;
using UnityEngine;

public class WizardSpawner : MonoBehaviour
{
    public List<Wizard> wizards = new();
    public GameObject wizardPrefab;

    private void Start()
    {
        SpawnWizard();
    }

    public void SpawnWizard()
    {
        GameObject wizardInstance = Instantiate(wizardPrefab, transform.position, Quaternion.identity, transform);
        Wizard wizard = wizardInstance.GetComponent<Wizard>();
        wizards.Add(wizard);
    }
}
