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
    public TMP_Text wizardInfoNameText;
    public TMP_Text wizardInfoLevelText;
    public TMP_Text wizardInfoDPSText;
    public TMP_Text nightCountText;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 1, ~LayerMask.NameToLayer("Wizard"));
        
        if (hit)
        {
            Wizard wiz = hit.transform.GetComponent<Wizard>();
            if (wiz)
            {
                wizardInfoCanvas.SetActive(true);
                
                wizardInfoNameText.text = wiz.spellData.name + " wizard";
                wizardInfoDPSText.text = wiz.GetComponent<DPSTracker>().GetDPS().ToString();
                wizardInfoLevelText.text = "level " + wiz.level;
                
                Vector3 worldPos = mousePos + Vector3.up * 2.0f;
                worldPos.z = 0;
                wizardInfoCanvas.transform.position = worldPos;
            }
            else
            {
                wizardInfoCanvas.SetActive(false);
            }
        }
        else
        {
            wizardInfoCanvas.SetActive(false);
        }
    }
}
