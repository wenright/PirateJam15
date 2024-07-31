using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public Transform damageTextParent;
    public Transform projectileParent;
    public Transform audioInstanceParent;
    public MonsterSpawner monsterSpawner;
    [ReadOnly] public int nightCount;

    public enum State
    {
        LOADING,
        AWAITING_START,
        NIGHTTIME,
        HIRING,
        SHOPPING,
        GAME_OVER
    }
    public State state = State.LOADING;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SwitchState(State.HIRING);
    }

    private void Update()
    {
        if (state == State.NIGHTTIME && monsterSpawner.doneSpawning && monsterSpawner.transform.childCount == 0)
        {
            GameController.Instance.SwitchState(State.HIRING);
        }
    }
    
    public void SwitchState(State newState)
    {
        Debug.Log($"Switching to state {newState}");
        state = newState;

        switch (state)
        {
            case State.LOADING:
                break;
            case State.AWAITING_START:
                break;
            case State.NIGHTTIME:
                nightCount++;
                UIController.Instance.nightCountText.text = "Night " + nightCount;
                UIController.Instance.shopParent.SetActive(false);
                monsterSpawner.StartRound();
                
                break;
            case State.HIRING:
                float interestRate = UpgradeController.Instance.ownedUpgrades.Where(u => u.upgradeType == UpgradeData.UpgradeType.AddInterest).Sum(u => u.value);
                if (interestRate > 0)
                {
                    UpgradeController.Instance.AddGold((int)(UpgradeController.Instance.GetGold() * interestRate));
                }
                
                UIController.Instance.shopParent.SetActive(true);
                UpgradeController.Instance.RefreshWizardShop();

                break;
            case State.SHOPPING:
                UpgradeController.Instance.RefreshShop();
                
                break;
            default:
                break;
        }
    }

    public void GameOver()
    {
        UIController.Instance.gameOver.SetActive(true);
    }
}
