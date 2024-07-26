using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private const int numCreaturesToSpawn = 10;
    private const float spawnDistance = 15.0f;
    private const float spawnDelaySeconds = 2.0f;
    private const float spawnIntervalSeconds = 1.0f;

    public GameObject monsterPrefab;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelaySeconds);
            
            for (int i = 0; i < numCreaturesToSpawn; i++)
            {
                Instantiate(monsterPrefab, Random.insideUnitCircle.normalized * spawnDistance, Quaternion.identity);
                yield return new WaitForSeconds(spawnIntervalSeconds);
            }
        }
    }
}
