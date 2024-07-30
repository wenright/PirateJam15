using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public float numCreaturesToSpawn = 10;
    public float spawnDelaySeconds = 2.0f;
    public float spawnDistance = 12.0f;
    public float roundLengthSeconds = 10.0f;
    public float difficultyScaling = 1.2f;
    public bool doneSpawning;

    public GameObject monsterPrefab;

    private IEnumerator Spawn()
    {
        doneSpawning = false;
        
        yield return new WaitForSeconds(spawnDelaySeconds);
        
        for (int i = 0; i < Mathf.Round(numCreaturesToSpawn); i++)
        {
            Instantiate(monsterPrefab, Random.insideUnitCircle.normalized * spawnDistance, Quaternion.identity, transform);
            yield return new WaitForSeconds(roundLengthSeconds / numCreaturesToSpawn);
        }
        
        yield return new WaitForSeconds(1.5f);
        
        numCreaturesToSpawn *= difficultyScaling;
        // TODO spawn new enemies each night?
        doneSpawning = true;
    }

    public void StartRound()
    {
        StartCoroutine(Spawn());
    }
}
