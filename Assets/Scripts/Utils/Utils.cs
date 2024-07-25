using UnityEngine;

public class Utils : MonoBehaviour
{
    public static void SetLayerRecursively(GameObject parent, int layer)
    {
        parent.layer = layer;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            SetLayerRecursively(parent.transform.GetChild(i).gameObject, layer);
        }
    }

    public static Vector3 WorldToUIPosition(Vector3 worldPosition)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(worldPosition) - new Vector3(Screen.width, Screen.height, 0) / 2;

        return pos;
    }
    
    public static float AngleToPoint(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
