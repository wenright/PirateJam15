using UnityEngine;

[CreateAssetMenu(fileName = "NewStatusEffect", menuName = "Status Effect Data")]
public class StatusEffectData : ScriptableObject
{
    public Damageable.DamageType type;
    public float value;
    public bool hasDuration = true;
    [ShowIf("hasDuration", true)] public float duration;
    public bool isStackable;
    public int stacks = 1;
    [ReadOnly] public GameObject source;
}
