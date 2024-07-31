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
    
    public static void PlayOneShot(AudioClip clip, Vector3 position)
    {
        if (clip == null)
        {
            Debug.LogError("Audio clip is null!");
            return;
        }

        GameObject audioInstance = new()
        {
            name = "AudioInstance (" + clip.name + ")",
        };
        audioInstance.transform.position = position;
        audioInstance.AddComponent<DestroyAfter>().waitTime = 0.5f;
        AudioSource source = audioInstance.AddComponent<AudioSource>();
        source.transform.parent = GameController.Instance.audioInstanceParent;
        source.clip = clip;
        source.volume = 0.2f;
        source.pitch = Random.Range(0.9f, 1.1f);
        source.spatialBlend = 1;
        source.Play();
    }
    
    public static int GetXpNeeded(int level)
    {
        // Copying the runescape formula
        return (int) Mathf.Floor(720 * Mathf.Pow(2, level / 7.0f) + (1 / 8.0f) * level * (level - 1) - 795);
    }
    
    public static Transform FindClosestByTag(Vector3 position, string tag)
    {
        float nearestDistance = Mathf.Infinity;
        Transform nearestTransform = null;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject gameObject in gameObjects)
        {
            float dist = Vector3.Distance(position, gameObject.transform.position);
            if (gameObject != null && gameObject.gameObject.activeSelf && dist < nearestDistance)
            {
                nearestDistance = dist;
                nearestTransform = gameObject.transform;
            }
        }

        return nearestTransform;
    }
}
