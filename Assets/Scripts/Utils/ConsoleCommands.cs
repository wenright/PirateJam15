using UnityEngine;
using IngameDebugConsole;

public class ConsoleCommands : MonoBehaviour
{
    [ConsoleMethod("motherlode", "Gives you a bunch of money")]
    public static void Motherlode()
    {
        Debug.Log("Here's some money");
        // ScoringManager.AddMoney(999, Vector3.zero);
    }
}