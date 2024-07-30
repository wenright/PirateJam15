using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public TMP_Text goldText;
    public Transform shopUpgradeCardParent;
    public GameObject shopParent;
    public GameObject wizardInfoCanvas;
    public Button wizardInfoCloseButton;
    public TMP_Text wizardInfoDPSText;
    public GameObject wizardUpgradeCanvas;
    public Button wizardUpgradeCloseButton;
    public TMP_Text nightCountText;

    [ReadOnly] public Wizard selectedWizard;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        wizardInfoCloseButton.onClick.AddListener(CloseWizardInfo);
        wizardUpgradeCloseButton.onClick.AddListener(CloseWizardUpgrade);
    }

    private void Update()
    {
        if (selectedWizard)
        {
            if (wizardInfoCanvas.activeSelf)
            {
                wizardInfoDPSText.text = selectedWizard.GetComponent<DPSTracker>().GetDPS().ToString();
            }
            else
            {
                
            }
        }
    }

    public void ShowWizardInfo(Wizard wizard)
    {
        selectedWizard = wizard;
        wizardInfoCanvas.SetActive(true);
    }

    public void ShowWizardUpgrade(Wizard wizard)
    {
        selectedWizard = wizard;
        wizardUpgradeCanvas.SetActive(true);
    }

    private void CloseWizardInfo()
    {
        wizardInfoCanvas.SetActive(false);
    }

    private void CloseWizardUpgrade()
    {
        wizardUpgradeCanvas.SetActive(false);
    }
}
