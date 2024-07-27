using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public Transform damageTextParent;
    public Transform projectileParent;
    public Transform audioInstanceParent;

    private void Awake()
    {
        Instance = this;
    }
}
