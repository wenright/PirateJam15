using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public Transform damageTextParent;
    public Transform projectileParent;
    public Transform audioInstanceParent;

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
            default:
                break;
        }
    }
}
