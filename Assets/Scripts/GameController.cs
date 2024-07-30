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
        // TODO Probably should have a start screen
        SwitchState(State.NIGHTTIME);
    }

    private void Update()
    {
        if (state == State.NIGHTTIME && monsterSpawner.doneSpawning && monsterSpawner.transform.childCount == 0)
        {
            GameController.Instance.SwitchState(State.SHOPPING);
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
            case State.SHOPPING:
                UIController.Instance.shopParent.SetActive(true);
                UpgradeController.Instance.RefreshShop();
                break;
            default:
                break;
        }
    }
}
