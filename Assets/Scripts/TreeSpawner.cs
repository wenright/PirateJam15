using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public Sprite treeSprite;
    public int numTrees = 150;
    public float treeRadius = 10.0f;

    private void Start()
    {
        for (int i = 0; i < numTrees; i++)
        {
            float angle = 2 * Mathf.PI * i / numTrees;
            GameObject treeInstance = new GameObject
            {
                transform =
                {
                    position = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * (treeRadius + Random.Range(0.0f, 0.5f)),
                    parent = transform
                },
                name = "Tree" + i
            };
            SpriteRenderer spriteRenderer = treeInstance.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = treeSprite;
            spriteRenderer.sortingOrder = 999;
        }
    }
}
