using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public TMP_Text goldText;
    public Transform shopUpgradeCardParent;
    public GameObject shopParent;
    public GameObject wizardCanvas;

    private void Awake()
    {
        Instance = this;
    }
}
