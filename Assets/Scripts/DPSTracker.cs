using System.Collections.Generic;
using UnityEngine;

public class DPSTracker : MonoBehaviour
{
    private class DamageEntry
    {
        public readonly float amount;
        public readonly float time;

        public DamageEntry(float amount, float time)
        {
            this.amount = amount;
            this.time = time;
        }
    }

    private readonly List<DamageEntry> damageEntries = new();
    private const float timeWindowSeconds = 1.0f;

    private void Update()
    {
        damageEntries.RemoveAll(e => Time.time - e.time > timeWindowSeconds);
    }

    public void Track(float amount)
    {
        damageEntries.Add(new DamageEntry(amount, Time.time));
    }

    public float GetDPS()
    {
        float total = 0;

        foreach (DamageEntry damageEntry in damageEntries)
        {
            if (Time.time - damageEntry.time <= timeWindowSeconds)
            {
                total += damageEntry.amount;
            }
        }

        return total / timeWindowSeconds;
    }
}
